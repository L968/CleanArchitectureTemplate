namespace CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;

public sealed record GetProductsQuery(int Page, int PageSize) : IRequest<PaginatedList<GetProductsResponse>>;
