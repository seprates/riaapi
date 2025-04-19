using RIA.API.Models;

namespace RIA.API.Data;

public interface ICustomerRepo
{
    Task<bool> SaveChangesAsync();
    Task<Customer?> GetCustomerByIdAsync(int id);
    Task<bool> CustomerExistsAsync(int id);
    Task<IEnumerable<Customer?>?> GetAllCustomersAsync();
    Task CreateCustomerAsync(Customer customer);
}