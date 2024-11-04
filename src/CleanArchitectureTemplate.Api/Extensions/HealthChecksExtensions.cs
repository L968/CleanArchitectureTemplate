using CleanArchitectureTemplate.Infrastructure.Database;

namespace CleanArchitectureTemplate.Api.Extensions;

internal static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<ProductsDbContext>();

        return services;
    }
}
