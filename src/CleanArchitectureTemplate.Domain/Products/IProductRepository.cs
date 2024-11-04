namespace CleanArchitectureTemplate.Domain.Products;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Product?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Product Create(Product product);
    void Update(Product product);
    void Delete(Product product);
}
