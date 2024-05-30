using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore.HealthChecks;

public class PrismAspNetCoreHealthChecksModule: PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<StartupHealthCheck>();
    }
}