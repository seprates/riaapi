using RIA.API.Models;

namespace RIA.API.Data;
public class CustomerRepo(AppDbContext dbContext) : ICustomerRepo
{
    private readonly AppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task CreateCustomerAsync(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        await _dbContext.Customers.AddAsync(customer);
    }

    public async Task<bool> CustomerExistsAsync(int id) => 
        await GetCustomerByIdAsync(id) != null;

    public async Task<IEnumerable<Customer?>?> GetAllCustomersAsync() =>
         await Task.FromResult(_dbContext.Customers.AsEnumerable());

    public async Task<Customer?> GetCustomerByIdAsync(int id) =>
        await _dbContext.Customers.FindAsync(id);

    public async Task<bool> SaveChangesAsync() =>
        await _dbContext.SaveChangesAsync() >= 0;
}