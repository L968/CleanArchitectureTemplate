﻿using CleanArchitectureTemplate.Application.Abstractions;
using CleanArchitectureTemplate.Domain.Products;
using CleanArchitectureTemplate.Infrastructure.Database;
using CleanArchitectureTemplate.Infrastructure.Extensions;
using CleanArchitectureTemplate.Infrastructure.Products;
using CleanArchitectureTemplate.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionStringOrThrow("cleanarchitecturetemplate-mysqldb")!;

        var serverVersion = ServerVersion.AutoDetect(databaseConnectionString);

        services.AddDbContext<AppDbContext>(options =>
            options
                .UseMySql(
                    databaseConnectionString,
                    serverVersion,
                    mysqlOptions => mysqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
        );

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
