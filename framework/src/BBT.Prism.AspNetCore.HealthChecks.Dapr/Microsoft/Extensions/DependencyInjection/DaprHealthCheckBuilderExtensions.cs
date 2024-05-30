using System;
using System.Collections.Generic;
using BBT.Prism.AspNetCore.HealthChecks.Dapr;
using BBT.Prism.Dapr;
using BBT.Prism.Threading;
using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class DaprHealthCheckBuilderExtensions
{
    private const string Name = DaprHealthCheck.Name;
    
    public static IHealthChecksBuilder AddDapr(
        this IHealthChecksBuilder builder,
        DaprClient daprClient,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            new DaprHealthCheck(daprClient),
            failureStatus,
            tags,
            timeout));
    }
    
    public static IHealthChecksBuilder AddDapr(
        this IHealthChecksBuilder builder,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            sp => new DaprHealthCheck( AsyncHelper.RunSync(() => sp.GetRequiredService<IPrismDaprClientFactory>().CreateAsync())),
            failureStatus,
            tags,
            timeout));
    }
}