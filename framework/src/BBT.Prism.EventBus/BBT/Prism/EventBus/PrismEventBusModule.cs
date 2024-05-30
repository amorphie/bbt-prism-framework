using System;
using System.Collections.Generic;
using BBT.Prism.EventBus.Abstractions;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Modularity;
using BBT.Prism.Reflection;
using BBT.Prism.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.EventBus;

[Modules(
    typeof(PrismEventBusAbstractionsModule)
)]
public class PrismEventBusModule : PrismModule
{
    public override void PreConfigureServices(ModuleConfigurationContext context)
    {
        AddEventHandlers(context.Services);
    }

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<IEventHandlerInvoker, EventHandlerInvoker>();
        context.Services.AddSingleton<IIntegrationEventBus, LocalIntegrationEventBus>();
        context.Services.AddSingleton<IDomainEventBus, DomainEventBus>();
    }
    
    private static void AddEventHandlers(IServiceCollection services)
    {
        var domainHandlers = new List<Type>();
        var integrationHandlers = new List<Type>();

        services.OnRegistered(context =>
        {
            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IDomainEventHandler<>)))
            {
                domainHandlers.Add(context.ImplementationType);
            }

            if (ReflectionHelper.IsAssignableToGenericType(context.ImplementationType, typeof(IIntegrationEventHandler<>)))
            {
                integrationHandlers.Add(context.ImplementationType);
            }
        });

        services.Configure<PrismDomainEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(domainHandlers);
        });

        services.Configure<PrismIntegrationEventBusOptions>(options =>
        {
            options.Handlers.AddIfNotContains(integrationHandlers);
        });
    }
}