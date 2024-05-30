using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore.Serilog;

[Modules(
    typeof(PrismAspNetCoreModule))]
public class PrismAspNetCoreSerilogModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<PrismSerilogMiddleware>();
    }
}