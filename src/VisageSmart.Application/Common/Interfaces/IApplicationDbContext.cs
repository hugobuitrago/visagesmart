using Microsoft.EntityFrameworkCore;
using VisageSmart.Domain.Entities;

namespace VisageSmart.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Service> Services { get; }
    DbSet<Product> Products { get; }
    DbSet<Professional> Professionals { get; }
    DbSet<Customer> Customers { get; }
    DbSet<User> Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
