using Aspire.Hosting;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<ParameterResource> mysqlPassword = builder.AddParameter("mysqlPassword", "root", secret: true);

IResourceBuilder<MySqlServerResource> mysql = builder.AddMySql("cleanarchitecturetemplate-mysql", password: mysqlPassword)
    .WithImageTag("9.2.0")
    .WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<MySqlDatabaseResource> mysqldb = mysql.AddDatabase("cleanarchitecturetemplate-mysqldb", "cleanarchitecturetemplate");

builder.AddProject<Projects.CleanArchitectureTemplate_Api>("cleanarchitecturetemplate-api")
    .WithReference(mysqldb)
    .WaitFor(mysqldb);

builder.AddProject<Projects.CleanArchitectureTemplate_MigrationService>("cleanarchitecturetemplate-migrationservice")
    .WithReference(mysqldb)
    .WaitFor(mysqldb);

await builder.Build().RunAsync();
