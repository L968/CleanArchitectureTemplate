using CleanArchitectureTemplate.Api.Extensions;
using CleanArchitectureTemplate.Api.Middleware;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Presentation.Endpoints;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddDocumentation();
builder.Services.AddHealthChecksConfiguration();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDocumentation();
}

app.MapEndpoints();

app.UseExceptionHandler(o => { });

app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

await app.RunAsync();
