using Microsoft.EntityFrameworkCore;
using RIA.API.Models;

namespace RIA.API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}