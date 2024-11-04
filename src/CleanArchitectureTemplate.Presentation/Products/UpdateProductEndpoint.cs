﻿using CleanArchitectureTemplate.Application.Features.Products.Commands.UpdateProduct;

namespace CleanArchitectureTemplate.Presentation.Products;

internal sealed class UpdateProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("product/{id:Guid}", async (Guid id, UpdateProductCommand command, ISender sender) =>
        {
            command.Id = id;
            await sender.Send(command);

            return Results.NoContent();
        })
        .WithTags(Tags.Products);
    }
}
