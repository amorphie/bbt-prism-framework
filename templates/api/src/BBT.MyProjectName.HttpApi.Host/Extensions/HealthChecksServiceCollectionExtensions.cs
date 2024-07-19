using System;
using BBT.MyProjectName.HealthChecks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.MyProjectName.Extensions;

public static class HealthChecksServiceCollectionExtensions
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        // Add your health checks here
        var healthChecksBuilder = services.AddHealthChecks();
        // Dapr is being used and can be activated.
        // Other HealthChecks can be added here.
        healthChecksBuilder
            .AddNpgSql(configuration.GetConnectionString("Default")!, tags: new[] { "PostgresDb" })
            .AddDapr(tags: new[] { "ready", "Dapr" })
            .AddDaprSecretStore(tags: new[] { "ready", "DaprSecretStore" })
            .AddDaprStateStore(tags: new[] { "ready", "DaprStateStore" })
            .AddDaprPubSub(tags: new[] { "ready", "DaprPubSub" });
        
        healthChecksBuilder
            .AddStartup(tags: new[] { "ready" });

        services.MapHealthCheckEndpoint(configuration["App:HealthCheckHost"] ?? "*:4200", "/health");
        return services;
    }

    private static IServiceCollection MapHealthCheckEndpoint(
        this IServiceCollection services,
        string hostName,
        string path)
    {
        services.Configure<PrismEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                endpointContext.Endpoints.MapHealthChecks(
                        new PathString(path.EnsureStartsWith('/')),
                        new HealthCheckOptions {
                            Predicate = _ => true,
                            AllowCachingResponses = false,
                            ResponseWriter = HealthCheckExtensions.WriteResponse
                        })
                    .RequireHost(hostName);

                endpointContext.Endpoints.MapHealthChecks(
                        new PathString(path.EnsureStartsWith('/') + "/ready"),
                        new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("ready") })
                    .RequireHost(hostName);

                endpointContext.Endpoints.MapHealthChecks(
                    new PathString(path.EnsureStartsWith('/') + "/live"),
                    new HealthCheckOptions { Predicate = _ => true }).RequireHost(hostName);
            });
        });

        return services;
    }
}