﻿using Microsoft.Extensions.Configuration;

namespace CleanArchitectureTemplate.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionStringOrThrow(this IConfiguration configuration, string name)
    {
        return configuration.GetConnectionString(name) ??
               throw new InvalidOperationException($"The connection string {name} was not found");
    }
}
