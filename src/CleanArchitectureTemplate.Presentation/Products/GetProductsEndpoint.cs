using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (IMediator mediator) =>
        {
            var query = new GetProductsQuery();
            IEnumerable<GetProductsResponse> response = await mediator.Send(query);

            return Results.Ok(response);
        })
        .WithTags(Tags.Products);
    }
}
