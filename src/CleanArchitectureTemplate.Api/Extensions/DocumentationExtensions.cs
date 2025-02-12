using Scalar.AspNetCore;

namespace CleanArchitectureTemplate.Api.Extensions;

internal static class DocumentationExtensions
{
    public static IServiceCollection AddDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();

        return services;
    }

    public static IApplicationBuilder UseDocumentation(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options => {
            options
                .WithTitle("VerticalSliceTemplate Api")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

            options.Servers = [];
        });

        return app;
    }
}
