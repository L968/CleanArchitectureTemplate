namespace CleanArchitectureTemplate.Domain.Products;

public sealed class Product
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

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
