using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BBT.Prism.AspNetCore.HealthChecks.Dapr;

public class DaprHealthCheck(DaprClient daprClient) : IHealthCheck
{
    internal const string Name = "MyProjectName-dapr";
    private readonly DaprClient _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await _daprClient.CheckHealthAsync(cancellationToken).ConfigureAwait(false)
            ? HealthCheckResult.Healthy()
            : new HealthCheckResult(context.Registration.FailureStatus);
    }
}