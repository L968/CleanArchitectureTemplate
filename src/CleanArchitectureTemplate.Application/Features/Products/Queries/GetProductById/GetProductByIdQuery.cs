namespace CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<GetProductByIdResponse>>;
