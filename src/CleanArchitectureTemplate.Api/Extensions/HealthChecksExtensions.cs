using CleanArchitectureTemplate.Infrastructure.Extensions;

namespace CleanArchitectureTemplate.Api.Extensions;

internal static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthChecksConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddMySql(configuration.GetConnectionStringOrThrow("cleanarchitecturetemplate-mysqldb"));

        return services;
    }
}
