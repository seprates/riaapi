using RIA.API.Models;

namespace RIA.API.Services;

public interface ICustomerService
{
    Task<bool> CustomerExistsAsync(int id);
    Task<IEnumerable<Customer?>?> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task CreateCustomerAsync(Customer customer);
}