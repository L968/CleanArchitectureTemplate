using System.Text.Json.Serialization;

namespace CleanArchitectureTemplate.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommand : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
