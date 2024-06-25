using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BBT.Prism.AspNetCore.HealthChecks.Dapr;

public class DaprPubSubHealthCheck(DaprClient daprClient) : IHealthCheck
{
    internal const string Name = "myprojectname-pubsub";
    
    private readonly DaprClient _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var testTopic = "MyProjectName-healthcheck-topic";
            var testMessage = new { Message = "test" };
            await _daprClient.PublishEventAsync(Name, testTopic, testMessage, cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch
        {
            return new HealthCheckResult(context.Registration.FailureStatus);
        }
    }
}