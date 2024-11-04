using CleanArchitectureTemplate.Application.Features.Products.Commands.CreateProduct;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class CreateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("product", async (CreateProductCommand command, ISender sender) =>
        {
            CreateProductResponse response = await sender.Send(command);

            return Results.Created($"/product/{response.Id}", response);
        })
        .WithTags(Tags.Products);
    }
}
