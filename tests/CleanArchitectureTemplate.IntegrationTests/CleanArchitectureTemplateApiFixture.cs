using Aspire.Hosting;
using CleanArchitectureTemplate.Infrastructure;

namespace CleanArchitectureTemplate.IntegrationTests;

public sealed class CleanArchitectureTemplateApiFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    private ResourceNotificationService? _resourceNotificationService;
    public HttpClient? HttpClient { get; private set; }

    public async Task InitializeAsync()
    {
        // Arrange
        IDistributedApplicationTestingBuilder appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.CleanArchitectureTemplate_Aspire_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _app = await appHost.BuildAsync();
        _resourceNotificationService = _app.Services.GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();

        // Create HttpClient and wait for the resource to be running
        HttpClient = _app.CreateHttpClient(ServiceNames.Api);
        await _resourceNotificationService.WaitForResourceAsync(ServiceNames.Api, KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
    }

    public async Task DisposeAsync()
    {
        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}
