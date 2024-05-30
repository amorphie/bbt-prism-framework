using System;
using System.Collections.Generic;
using BBT.Prism.AspNetCore.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Microsoft.Extensions.DependencyInjection;

public static class StartupHealthCheckBuilderExtensions
{
    private const string Name = StartupHealthCheck.Name;
    
    public static IHealthChecksBuilder AddStartup(
        this IHealthChecksBuilder builder,
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(new HealthCheckRegistration(
            name ?? Name,
            sp => sp.GetRequiredService<StartupHealthCheck>(),
            failureStatus,
            tags,
            timeout));
    }
}