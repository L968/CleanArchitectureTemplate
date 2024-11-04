namespace CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery : IRequest<IEnumerable<GetProductsResponse>>;
