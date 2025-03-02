using CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;

namespace CleanArchitectureTemplate.Presentation.Products.v1;

internal sealed class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("product", async (CreateProductCommand command, ISender sender, CancellationToken cancellationToken) =>
        {
            CreateProductResponse response = await sender.Send(command, cancellationToken);

            return Results.Created($"/product/{response.Id}", response);
        })
        .WithTags(Tags.Products)
        .MapToApiVersion(1);
    }
}
