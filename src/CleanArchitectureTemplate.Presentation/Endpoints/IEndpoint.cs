using Microsoft.AspNetCore.Routing;

namespace CleanArchitectureTemplate.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
