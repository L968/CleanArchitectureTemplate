using CleanArchitectureTemplate.Infrastructure;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> postgresPassword = builder.AddParameter("postgresPassword", "root", secret: true);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres(ServiceNames.Postgres, password: postgresPassword)
    .WithImageTag("17.4")
    .WithPgWeb()
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<PostgresDatabaseResource> productsDb = postgres.AddDatabase(ServiceNames.PostgresDb, ServiceNames.DatabaseName);

builder.AddProject<Projects.CleanArchitectureTemplate_Api>(ServiceNames.Api)
    .WithReference(productsDb)
    .WaitFor(productsDb);

builder.AddProject<Projects.CleanArchitectureTemplate_MigrationService>(ServiceNames.MigrationService)
    .WithReference(productsDb)
    .WaitFor(productsDb);

await builder.Build().RunAsync();
