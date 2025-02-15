using Scalar.AspNetCore;

namespace CleanArchitectureTemplate.Api.Extensions;

internal static class DocumentationExtensions
{
    public static IApplicationBuilder UseDocumentation(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options => {
            options
                .WithTitle("CleanArchitectureTemplate Api")
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

            options.Servers = [];
        });

        return app;
    }
}
