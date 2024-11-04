using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class GetProductByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("product/{id:Guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetProductByIdQuery(id);
            GetProductByIdResponse? response = await sender.Send(query);

            return response is not null
                ? Results.Ok(response)
                : Results.NotFound();
        })
        .WithTags(Tags.Products);
    }
}
