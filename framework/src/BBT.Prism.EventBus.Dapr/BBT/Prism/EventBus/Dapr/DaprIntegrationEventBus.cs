using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.Dapr;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Guids;
using BBT.Prism.Threading;
using BBT.Prism.Timing;
using BBT.Prism.Tracing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.EventBus.Dapr;

public sealed class DaprIntegrationEventBus(
    IServiceScopeFactory serviceScopeFactory,
    IEventHandlerInvoker eventHandlerInvoker,
    IGuidGenerator guidGenerator,
    IClock clock,
    IDomainEventBus domainEventBus,
    ICorrelationIdProvider correlationIdProvider,
    IOptions<PrismIntegrationEventBusOptions> integrationEventBusOptions,
    IDaprSerializer daprSerializer,
    IPrismDaprClientFactory daprClientFactory,
    IOptions<PrismDaprEventBusOptions> daprEventBusOptions)
    : IntegrationEventBusBase(
        serviceScopeFactory,
        guidGenerator,
        clock,
        eventHandlerInvoker,
        domainEventBus,
        correlationIdProvider,
        integrationEventBusOptions)
{
    private PrismDaprEventBusOptions DaprEventBusOptions => daprEventBusOptions.Value;
    private ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; } = new();
    private ConcurrentDictionary<string, Type> EventTypes { get; } = new();

    public void Initialize()
    {
        SubscribeHandlers(IntegrationEventBusOptions.Handlers);
    }

    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        var handlerFactories = GetOrCreateHandlerFactories(eventType);

        if (factory.IsInFactories(handlerFactories))
        {
            return NullDisposable.Instance;
        }

        handlerFactories.Add(factory);

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    public override void Unsubscribe<TEvent>(Func<TEvent, Task> action)
    {
        Check.NotNull(action, nameof(action));

        GetOrCreateHandlerFactories(typeof(TEvent))
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                    {
                        var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                        if (singleInstanceFactory == null)
                        {
                            return false;
                        }

                        var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                        if (actionHandler == null)
                        {
                            return false;
                        }

                        return actionHandler.Action == action;
                    });
            });
    }

    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        (factory as SingleInstanceHandlerFactory)!.HandlerInstance == handler
                );
            });
    }

    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    protected async override Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishToDaprAsync(eventType, eventData, null, CorrelationIdProvider.Get());
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
        }

        return handlerFactoryList.ToArray();
    }

    public async Task TriggerHandlersAsync(Type eventType, object eventData, string? messageId = null,
        string? correlationId = null)
    {
        using (CorrelationIdProvider.Change(correlationId))
        {
            await TriggerHandlersDirectAsync(eventType, eventData);
        }
    }

    protected override byte[] Serialize(object eventData)
    {
        return daprSerializer.Serialize(eventData);
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(
            eventType,
            type =>
            {
                var eventName = EventNameAttribute.GetNameOrDefault(type);
                EventTypes.GetOrAdd(eventName, eventType);
                return new List<IEventHandlerFactory>();
            }
        );
    }

    public Type GetEventType(string eventName)
    {
        return EventTypes.GetOrDefault(eventName)!;
    }

    private async Task PublishToDaprAsync(Type eventType, object eventData, Guid? messageId = null,
        string? correlationId = null)
    {
        await PublishToDaprAsync(EventNameAttribute.GetNameOrDefault(eventType), eventData, messageId, correlationId);
    }

    private async Task PublishToDaprAsync(string eventName, object eventData, Guid? messageId = null,
        string? correlationId = null)
    {
        var client = await daprClientFactory.CreateAsync();
        var data = new PrismDaprEventData(DaprEventBusOptions.PubSubName, eventName,
            (messageId ?? GuidGenerator.Create()).ToString("N"), daprSerializer.SerializeToString(eventData),
            correlationId);
        await client.PublishEventAsync(pubsubName: DaprEventBusOptions.PubSubName, topicName: eventName, data: data);
    }

    private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
    {
        //Should trigger same type
        if (handlerEventType == targetEventType)
        {
            return true;
        }

        //Should trigger for inherited types
        if (handlerEventType.IsAssignableFrom(targetEventType))
        {
            return true;
        }

        return false;
    }
}