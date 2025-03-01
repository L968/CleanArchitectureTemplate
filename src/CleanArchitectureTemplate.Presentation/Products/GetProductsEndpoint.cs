using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class GetProductsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
                ISender sender,
                CancellationToken cancellationToken,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10) =>
            {
                var query = new GetProductsQuery(page, pageSize);
                PaginatedList<GetProductsResponse> response = await sender.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .WithTags(Tags.Products);
    }
}
