using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RIA.API.Dtos;
using RIA.API.Models;
using RIA.API.Services;
using RIA.API.Validations;

namespace RIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService customerService,
    IValidationService validationService, IMapper mapper, ILogger<CustomersController> logger) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
    private readonly IValidationService _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Returns all registered Customers ordered by last names, then first names
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerReadDto>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(_mapper.Map<IEnumerable<CustomerReadDto>>(customers));
    }

    /// <summary>
    /// Returns an individual Customer with provided Id
    /// </summary>
    /// <param name="id">The Customer Id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerReadDto>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(_mapper.Map<CustomerReadDto>(customer));
    }

    /// <summary>
    /// Creates one or more Customer(s)
    /// </summary>
    /// <param name="customerDtos">Array of Customer info.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<CustomerReadDto>> CreateCustomer([FromBody] IEnumerable<CustomerCreateDto> customerDtos)
    {
        try
        {
            if (customerDtos?.Any() != true)
                return BadRequest(new ValidationFailedDto(errors: ["Request body cannot be empty."]));

            foreach (var customer in customerDtos)
            {
                if (!_validationService.TryValidate<CustomerCreateDto, CustomerCreateValidator>(customer, out var errors))
                {
                    return BadRequest(new ValidationFailedDto(errors: errors));
                }
                if (await _customerService.CustomerExistsAsync(customer.Id))
                {
                    return BadRequest(new ValidationFailedDto(errors: [$"Customer with Id: {customer.Id} already exists."]));
                }
            }

            var customers = _mapper.Map<IEnumerable<Customer>>(customerDtos);
            List<int> failedCustomerIds = [];
            List<int> uniqueCustomerIds = [];

            foreach (var customer in customers)
            {
                try
                {

                    if (await _customerService.CustomerExistsAsync(customer.Id))
                        failedCustomerIds.Add(customer.Id);
                    else
                    {
                        uniqueCustomerIds.Add(customer.Id);
                        await _customerService.CreateCustomerAsync(customer);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"--> An error occurred while creating customer with Id: {customer.Id}");
                }
            }

            return Ok(new
            {
                SucceededIds = uniqueCustomerIds,
                FailedIds = failedCustomerIds
            });
        }
        catch (Exception e)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(customerDtos);
            _logger.LogCritical(exception: e, message: "--> An error occurred while creating customer(s)", args: json);
            // [Note]: PreconditionFailed(412) used instead of 500 to avoid service restarts in Azure
            return StatusCode((int)HttpStatusCode.PreconditionFailed);
        }
    }
}