using System;
using System.Collections.Generic;
using BBT.Prism.AspNetCore.HealthChecks.Dapr;
using BBT.Prism.Dapr;
using BBT.Prism.Threading;
using Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class DaprSecretStoreHealthCheckBuilderExtensions
{
    private const string Name = DaprSecretStoreHealthCheck.Name;
    
    public static IHealthChecksBuilder AddDaprSecretStore(
        this IHealthChecksBuilder builder,
        DaprClient daprClient,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            new DaprSecretStoreHealthCheck(daprClient),
            failureStatus,
            tags,
            timeout));
    }
    
    public static IHealthChecksBuilder AddDaprSecretStore(
        this IHealthChecksBuilder builder,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            sp => new DaprSecretStoreHealthCheck(AsyncHelper.RunSync(() => sp.GetRequiredService<IPrismDaprClientFactory>().CreateAsync())),
            failureStatus,
            tags,
            timeout));
    }
}