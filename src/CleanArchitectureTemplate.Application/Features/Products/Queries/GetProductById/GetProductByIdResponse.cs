namespace CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdResponse(
    Guid Id,
    string Name,
    decimal Price
);
