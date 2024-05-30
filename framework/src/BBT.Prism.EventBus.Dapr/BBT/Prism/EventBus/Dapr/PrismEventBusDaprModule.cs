using BBT.Prism.Dapr;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BBT.Prism.EventBus.Dapr;

[Modules(
    typeof(PrismEventBusModule),
    typeof(PrismDaprModule)
)]
public class PrismEventBusDaprModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<DaprIntegrationEventBus>();
        context.Services.Replace(ServiceDescriptor.Singleton<IIntegrationEventBus, DaprIntegrationEventBus>());
    }
    
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context
            .ServiceProvider
            .GetRequiredService<DaprIntegrationEventBus>()
            .Initialize();
    }
}