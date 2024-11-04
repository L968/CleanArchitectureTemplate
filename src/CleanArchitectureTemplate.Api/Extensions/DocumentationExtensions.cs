using Scalar.AspNetCore;

namespace CleanArchitectureTemplate.Api.Extensions;

internal static class DocumentationExtensions
{
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IApplicationBuilder UseDocumentation(this WebApplication app)
    {
        app.UseSwagger(options =>
        {
            options.RouteTemplate = "openapi/{documentName}.json";
        });

        app.MapScalarApiReference(options => {
            options
                .WithTitle("CleanArchitectureTemplate Api")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        return app;
    }
}
