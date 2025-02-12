using CleanArchitectureTemplate.Application.Features.Products.Commands.DeleteProduct;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class DeleteProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("product/{id:Guid}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            await mediator.Send(new DeleteProductCommand(id), cancellationToken);
            return Results.NoContent();
        })
        .WithTags(Tags.Products);
    }
}
