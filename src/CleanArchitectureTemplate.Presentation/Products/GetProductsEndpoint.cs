using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();
            IEnumerable<GetProductsResponse> response = await sender.Send(query);

            return Results.Ok(response);
        })
        .WithTags(Tags.Products);
    }
}
