using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductHandler(
    IAppDbContext dbContext,
    ILogger<CreateProductHandler> logger
) : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool existingProduct = await dbContext.Products.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (existingProduct)
        {
            throw new AppException(ProductErrors.ProductAlreadyExists(request.Name));
        }

        var product = new Product(
            request.Name,
            request.Price
        );

        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created {@Product}", product);

        return new CreateProductResponse(
            product.Id,
            product.Name,
            product.Price
        );
    }
}
