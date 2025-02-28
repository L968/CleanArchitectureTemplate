using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
