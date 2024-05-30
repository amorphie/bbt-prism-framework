using BBT.Prism.Dapr;
using BBT.Prism.Modularity;

namespace BBT.Prism.AspNetCore.HealthChecks.Dapr;

[Modules(
    typeof(PrismAspNetCoreHealthChecksModule),
    typeof(PrismDaprModule)
    )]
public class PrismAspNetCoreHealthChecksDaprModule: PrismModule
{
}