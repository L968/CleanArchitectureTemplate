using CleanArchitectureTemplate.Application.Abstractions;
using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;

internal sealed class CreateProductHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    ILogger<CreateProductHandler> logger
) : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product existingProduct = await repository.GetByNameAsync(request.Name, cancellationToken);

        if (existingProduct is not null)
        {
            throw new AppException(ProductErrors.ProductAlreadyExists(request.Name));
        }

        var product = new Product(
            request.Name,
            request.Price
        );

        repository.Create(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully created {@Product}", product);

        return new CreateProductResponse(
            product.Id,
            product.Name,
            product.Price
        );
    }
}
