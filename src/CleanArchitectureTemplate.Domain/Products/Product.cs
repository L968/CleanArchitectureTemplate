namespace CleanArchitectureTemplate.Domain.Products;

public sealed class Product : IAuditableEntity
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }

    private Product() { }

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    public void Update(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}
