using RIA.API.Data;
using RIA.API.Models;

namespace RIA.API.Services;

public class CustomerService : ICustomerService
{
    private readonly IList<Customer> _customers = [];
    private readonly ICustomerRepo _customerRepo;

    public CustomerService(ICustomerRepo customerRepo)
    {
        _customerRepo = customerRepo ?? throw new ArgumentNullException(nameof(customerRepo));
        PopulateCustomers();
    }

    public async Task<bool> CustomerExistsAsync(int id) => await _customerRepo.CustomerExistsAsync(id);

    public async Task<IEnumerable<Customer?>?> GetAllCustomersAsync() => await Task.FromResult(_customers);

    public async Task<Customer?> GetCustomerByIdAsync(int id) => await _customerRepo.GetCustomerByIdAsync(id);

    public async Task CreateCustomerAsync(Customer customer)
    {
        await _customerRepo.CreateCustomerAsync(customer);
        await _customerRepo.SaveChangesAsync();
        int idx = GetInsertionIndex(_customers, customer);
        _customers.Insert(idx, customer);
    }

    #region Private Methods
    /// <summary>
    /// Populates all the existing customers from DB and orders them by last name, first name
    /// </summary>
    private void PopulateCustomers()
    {
        var customers = _customerRepo.GetAllCustomersAsync().Result;
        foreach (var customer in customers)
        {
            int idx = GetInsertionIndex(_customers, customer);
            _customers.Insert(idx, customer);
        }
    }

    /// <summary>
    /// Finds the proper index where the new customer can be inserted 
    /// in the list of customers maintaining the order by lastname, firstname rule
    /// </summary>
    /// <param name="customers">List of Customers</param>
    /// <param name="customer">New customer to be inserted</param>
    /// <returns></returns>
    private static int GetInsertionIndex(IList<Customer> customers, Customer customer)
    {
        int l = 0;
        int r = customers?.Count ?? 0;
        while (l < r)
        {
            int m = l + (r - l) / 2;
            var midCustomer = customers[m];
            if (string.Compare(customer.LastName, midCustomer.LastName) == 0)
            {
                if (string.Compare(customer.FirstName, midCustomer.FirstName) <= 0)
                {
                    r = m;
                }
                else
                {
                    l = m + 1;
                }
            }
            else if (string.Compare(customer.LastName, midCustomer.LastName) < 0)
            {
                r = m;
            }
            else
            {
                l = m + 1;
            }
        }
        return l;
    }
    #endregion Private Methods
}