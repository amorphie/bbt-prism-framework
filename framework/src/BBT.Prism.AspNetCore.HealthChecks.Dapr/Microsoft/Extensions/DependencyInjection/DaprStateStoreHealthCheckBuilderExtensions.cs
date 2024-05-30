using System;
using System.Collections.Generic;
using BBT.Prism.AspNetCore.HealthChecks.Dapr;
using BBT.Prism.Dapr;
using BBT.Prism.Threading;
using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class DaprStateStoreHealthCheckBuilderExtensions
{
    private const string Name = DaprStateStoreHealthCheck.Name;

    public static IHealthChecksBuilder AddDaprStateStore(
        this IHealthChecksBuilder builder,
        DaprClient daprClient,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            new DaprStateStoreHealthCheck(daprClient),
            failureStatus,
            tags,
            timeout));
    }

    public static IHealthChecksBuilder AddDaprStateStore(
        this IHealthChecksBuilder builder,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            sp => new DaprStateStoreHealthCheck(AsyncHelper.RunSync(() => sp.GetRequiredService<IPrismDaprClientFactory>().CreateAsync())),
            failureStatus,
            tags,
            timeout));
    }
}