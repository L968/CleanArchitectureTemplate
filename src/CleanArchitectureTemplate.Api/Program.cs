using CleanArchitectureTemplate.Api.Extensions;
using CleanArchitectureTemplate.Api.Middleware;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Aspire.ServiceDefaults;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Presentation.Endpoints;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHealthChecksConfiguration(builder.Configuration);

builder.Services.AddOpenApi();

builder.Host.AddSerilogLogging();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

app.MapDefaultEndpoints();

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDocumentation();
}

app.UseExceptionHandler(o => { });

app.UseHttpsRedirection();

await app.RunAsync();
