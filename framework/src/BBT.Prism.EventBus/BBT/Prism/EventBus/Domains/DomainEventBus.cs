using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Integrations;
using BBT.Prism.Reflection;
using BBT.Prism.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BBT.Prism.EventBus.Domains;

public class DomainEventBus: EventBusBase, IDomainEventBus
{
     /// <summary>
    /// Reference to the Logger.
    /// </summary>
    public ILogger<DomainEventBus> Logger { get; set; }
    protected PrismDomainEventBusOptions Options { get; }

    private ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }

    public DomainEventBus(
        IOptions<PrismDomainEventBusOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        IEventHandlerInvoker eventHandlerInvoker)
        : base(serviceScopeFactory, eventHandlerInvoker)
    {
        Options = options.Value;
        Logger = NullLogger<DomainEventBus>.Instance;

        HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
        SubscribeHandlers(Options.Handlers);
    }
    /// <inheritdoc/>
    public IDisposable Subscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    /// <inheritdoc/>
    public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
                {
                    if (!factory.IsInFactories(factories))
                    {
                        factories.Add(factory);
                    }
                }
            );

        return new EventHandlerFactoryUnregistrar(this, eventType, factory);
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandler handler)
    {
        GetOrCreateHandlerFactories(eventType)
            .Locking(factories =>
            {
                factories.RemoveAll(
                    factory =>
                        factory is SingleInstanceHandlerFactory &&
                        ((factory as SingleInstanceHandlerFactory)!).HandlerInstance == handler
                );
            });
    }

    /// <inheritdoc/>
    public override void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
    }

    /// <inheritdoc/>
    public override void UnsubscribeAll(Type eventType)
    {
        GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
    }

    protected override async Task PublishToEventBusAsync(Type eventType, object eventData)
    {
        await PublishAsync(new DomainEventMessage(Guid.NewGuid(), eventData, eventType));
    }

    public async Task PublishAsync(DomainEventMessage localEventMessage)
    {
        await TriggerHandlersAsync(localEventMessage.EventType, localEventMessage.EventData);
    }

    protected override IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
    {
        var handlerFactoryList = new List<Tuple<IEventHandlerFactory, Type, int>>();
        foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
        {
            foreach (var factory in handlerFactory.Value)
            {
                handlerFactoryList.Add(new Tuple<IEventHandlerFactory, Type, int>(
                    factory,
                    handlerFactory.Key,
                    ReflectionHelper.GetAttributesOfMemberOrDeclaringType<DomainEventHandlerOrderAttribute>(factory.GetHandler().EventHandler.GetType()).FirstOrDefault()?.Order ?? 0));
            }
        }

        return handlerFactoryList.OrderBy(x => x.Item3).Select(x => new EventTypeWithEventHandlerFactories(x.Item2, new List<IEventHandlerFactory> {x.Item1})).ToArray();
    }

    private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
    {
        return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
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

    // Internal for unit testing
    internal Func<Type, object, Task>? OnEventHandleInvoking { get; set; }

    // Internal for unit testing
    protected async override Task InvokeEventHandlerAsync(IEventHandler eventHandler, object eventData, Type eventType)
    {
        if (OnEventHandleInvoking != null && eventType != typeof(IntegrationEventSent) && eventType != typeof(IntegrationEventReceived))
        {
            await OnEventHandleInvoking(eventType, eventData);
        }

        await base.InvokeEventHandlerAsync(eventHandler, eventData, eventType);
    }

    // Internal for unit testing
    internal Func<Type, object, Task>? OnPublishing { get; set; }

    // For unit testing
    public async override Task PublishAsync(
        Type eventType,
        object eventData)
    {
       // For unit testing
        if (OnPublishing != null && eventType != typeof(IntegrationEventSent) && eventType != typeof(IntegrationEventReceived))
        {
            await OnPublishing(eventType, eventData);
        }

        await PublishToEventBusAsync(eventType, eventData);
    }
}
