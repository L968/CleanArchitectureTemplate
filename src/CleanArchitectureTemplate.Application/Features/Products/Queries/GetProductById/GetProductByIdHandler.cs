using CleanArchitectureTemplate.Domain.Products;

namespace CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;

internal sealed class GetProductByIdHandler(
    IProductRepository repository,
    ILogger<GetProductByIdHandler> logger
) : IRequestHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            throw new AppException(ProductErrors.ProductNotFound(request.Id));
        }

        logger.LogInformation("Successfully retrieved  Product with Id {Id}", request.Id);

        return new GetProductByIdResponse(
            product.Id,
            product.Name,
            product.Price
        );
    }
}
