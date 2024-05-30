using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.AspNetCore.HealthChecks;
using BBT.Prism.Dapr;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BBT.MyProjectName.HealthChecks;

public class StartupBackgroundService(
    StartupHealthCheck healthCheck,
    IPrismDaprClientFactory daprClientFactory,
    ILogger<StartupBackgroundService> logger)
    : BackgroundService
{
    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var daprClient = await daprClientFactory.CreateAsync();
            await daprClient.WaitForSidecarAsync(stoppingToken);
            healthCheck.StartupCompleted = true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Dapr sidecar initialization failed.");
        }
    }
}