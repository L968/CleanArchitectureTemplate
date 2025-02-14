using CleanArchitectureTemplate.Infrastructure.Database;
using CleanArchitectureTemplate.MigrationService;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddMySqlDbContext<AppDbContext>("cleanarchitecturetemplate-mysqldb");

IHost host = builder.Build();
await host.RunAsync();
