using Microsoft.AspNetCore.Mvc;
using RIA.API.Services;

namespace RIA.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DenominationsController : ControllerBase
{
    private readonly IDenominationService _denominationService;
    private readonly IConfiguration _configuration;
    private readonly HashSet<int> _validAmounts;

    public DenominationsController(IDenominationService denominationService, IConfiguration configuration)
    {
        _denominationService = denominationService ?? throw new ArgumentNullException(nameof(denominationService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _validAmounts = [.. _configuration.GetSection("Denominations:ValidAmounts").Get<int[]>() ?? []];
    }

    /// <summary>
    /// Returns all possible combinations of bills that can be paid at ATM for the requested amount.
    /// </summary>
    /// <param name="amount">The amount of cash to be withdrawn; it must be among the valid amounts.</param>
    /// <returns></returns>
    [HttpGet]
    public ActionResult<IEnumerable<Dictionary<int, int>>> GetPaymentDenominations(int amount)
    {
        if (!ValidateAmount(amount)) return BadRequest($"Entered amount is invalid. Only the following amounts are accepted: [{string.Join(',', _validAmounts.AsEnumerable())}]");
        return Ok(_denominationService.GetDenominations(amount));
    }

    /// <summary>
    /// Returns available cartridges and valid amounts of cash to be withdrawn.
    /// </summary>
    /// <returns></returns>
    [HttpGet("info")]
    public ActionResult GetPaymentDenominationsInfo()
    {
        return Ok(new
        {
            Cartridges = _configuration.GetSection("Denominations:Cartridges").Get<int[]>(),
            ValidAmounts = _configuration.GetSection("Denominations:ValidAmounts").Get<int[]>()
        });
    }

    #region Private Methods
    private bool ValidateAmount(int amount) => _validAmounts.Contains(amount);
    #endregion Private Methods
}