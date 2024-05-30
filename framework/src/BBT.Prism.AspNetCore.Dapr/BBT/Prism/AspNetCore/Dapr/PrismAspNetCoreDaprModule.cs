using BBT.Prism.Dapr;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore.Dapr;

[Modules(
    typeof(PrismAspNetCoreModule),
    typeof(PrismDaprModule)
)]
public class PrismAspNetCoreDaprModule: PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<IDaprAppApiTokenValidator, DaprAppApiTokenValidator>();
    }
}