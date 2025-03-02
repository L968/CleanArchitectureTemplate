using CleanArchitectureTemplate.Application.Features.Products.Queries.GetProductById;

namespace CleanArchitectureTemplate.Presentation.Products.v1;

internal sealed class GetProductByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("product/{id:Guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetProductByIdQuery(id);
            GetProductByIdResponse? response = await sender.Send(query, cancellationToken);

            return response is not null
                ? Results.Ok(response)
                : Results.NotFound();
        })
        .WithTags(Tags.Products)
        .MapToApiVersion(1);
    }
}
