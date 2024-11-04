using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Products;

internal sealed class ProductRepository(ProductsDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return await context.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Products.FindAsync([id], cancellationToken);
    }

    public async Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Products.SingleOrDefaultAsync(p => p.Name == name, cancellationToken);
    }

    public Product Create(Product product)
    {
        context.Products.Add(product);
        return product;
    }

    public void Update(Product product)
    {
        context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        context.Products.Remove(product);
    }
}
