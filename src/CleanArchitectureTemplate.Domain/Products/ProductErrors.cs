namespace CleanArchitectureTemplate.Domain.Products;

public static class ProductErrors
{
    public static Error ProductAlreadyExists(string productName) =>
        new($"A product with name \"{productName}\" already exists.", ErrorType.Conflict);

    public static Error ProductNotFound(Guid productId) =>
        new($"The product with identifier \"{productId}\" was not found.", ErrorType.NotFound);
}
