using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.DependencyInjection;
using BBT.Prism.EventBus;
using BBT.Prism.EventBus.Dapr;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Modularity;
using Dapr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.AspNetCore.Dapr.EventBus;

[Modules(
    typeof(PrismAspNetCoreDaprModule),
    typeof(PrismEventBusDaprModule)
)]
public class PrismAspNetCoreDaprEventBusModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        var subscribeOptions = context.Services.ExecutePreConfiguredActions<PrismSubscribeOptions>();

        Configure<PrismEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                var rootServiceProvider =
                    endpointContext.ScopeServiceProvider.GetRequiredService<IRootServiceProvider>();
                subscribeOptions.SubscriptionsCallback = subscriptions =>
                {
                    var daprEventBusOptions =
                        rootServiceProvider.GetRequiredService<IOptions<PrismDaprEventBusOptions>>().Value;
                    foreach (var handler in rootServiceProvider
                                 .GetRequiredService<IOptions<PrismIntegrationEventBusOptions>>().Value.Handlers)
                    {
                        foreach (var @interface in handler.GetInterfaces().Where(x =>
                                     x.IsGenericType && x.GetGenericTypeDefinition() ==
                                     typeof(IIntegrationEventHandler<>)))
                        {
                            var eventType = @interface.GetGenericArguments()[0];
                            var eventName = EventNameAttribute.GetNameOrDefault(eventType);

                            if (subscriptions.Any(x =>
                                    x.PubsubName == daprEventBusOptions.PubSubName && x.Topic == eventName))
                            {
                                // Controllers with a [Topic] attribute can replace built-in event handlers.
                                continue;
                            }

                            var subscription = new PrismSubscription {
                                PubsubName = daprEventBusOptions.PubSubName,
                                Topic = eventName,
                                Route = PrismAspNetCoreDaprPubSubConsts.DaprEventCallbackUrl,
                                Metadata = new PrismMetadata { { PrismMetadata.RawPayload, "true" } }
                            };
                            subscriptions.Add(subscription);
                        }
                    }

                    return Task.CompletedTask;
                };

                endpointContext.Endpoints.MapPrismSubscribeHandler(subscribeOptions);
            });
        });
    }
}