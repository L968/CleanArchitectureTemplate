namespace CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    decimal Price
) : IRequest<Result<CreateProductResponse>>;
