using System;
using System.Reflection;
using System.Threading.Tasks;
using BBT.Prism.Collections;
using BBT.Prism.EventBus.Domains;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.EventBus.Integrations;

public class LocalIntegrationEventBus : IIntegrationEventBus
{
    private readonly IDomainEventBus _localEventBus;
    private IServiceScopeFactory ServiceScopeFactory { get; }
    protected PrismIntegrationEventBusOptions IntegrationEventBusOptions { get; }

    public LocalIntegrationEventBus(
        IDomainEventBus localEventBus,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<PrismIntegrationEventBusOptions> integrationEventBusOptions)
    {
        _localEventBus = localEventBus;
        ServiceScopeFactory = serviceScopeFactory;
        IntegrationEventBusOptions = integrationEventBusOptions.Value;
        Subscribe(integrationEventBusOptions.Value.Handlers);
        // For unit testing
        if (localEventBus is DomainEventBus eventBus)
        {
            eventBus.OnEventHandleInvoking = async (eventType, eventData) =>
            {
                await localEventBus.PublishAsync(new IntegrationEventReceived() {
                    Source = IntegrationEventSource.Direct,
                    EventName = EventNameAttribute.GetNameOrDefault(eventType),
                    EventData = eventData
                });
            };

            eventBus.OnPublishing = async (eventType, eventData) =>
            {
                await localEventBus.PublishAsync(new IntegrationEventSent() {
                    Source = IntegrationEventSource.Direct,
                    EventName = EventNameAttribute.GetNameOrDefault(eventType),
                    EventData = eventData
                });
            };
        }
    }

    public void Subscribe(ITypeList<IEventHandler> handlers)
    {
        foreach (var handler in handlers)
        {
            var interfaces = handler.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                {
                    continue;
                }

                var genericArgs = @interface.GetGenericArguments();
                if (genericArgs.Length == 1)
                {
                    Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                }
            }
        }
    }

    /// <inheritdoc/>
    public IDisposable Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : class
    {
        return Subscribe(typeof(TEvent), handler);
    }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return _localEventBus.Subscribe(action);
    }

    public IDisposable Subscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : class
    {
        return _localEventBus.Subscribe(handler);
    }

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
    {
        return _localEventBus.Subscribe<TEvent, THandler>();
    }

    public IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return _localEventBus.Subscribe(eventType, handler);
    }

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return _localEventBus.Subscribe<TEvent>(factory);
    }

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        return _localEventBus.Subscribe(eventType, factory);
    }

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        _localEventBus.Unsubscribe(action);
    }

    public void Unsubscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : class
    {
        _localEventBus.Unsubscribe(handler);
    }

    public void Unsubscribe(Type eventType, IEventHandler handler)
    {
        _localEventBus.Unsubscribe(eventType, handler);
    }

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        _localEventBus.Unsubscribe<TEvent>(factory);
    }

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {
        _localEventBus.Unsubscribe(eventType, factory);
    }

    public void UnsubscribeAll<TEvent>() where TEvent : class
    {
        _localEventBus.UnsubscribeAll<TEvent>();
    }

    public void UnsubscribeAll(Type eventType)
    {
        _localEventBus.UnsubscribeAll(eventType);
    }

    public Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class
    {
        return _localEventBus.PublishAsync(eventData);
    }

    public Task PublishAsync(Type eventType, object eventData)
    {
        return _localEventBus.PublishAsync(eventType, eventData);
    }
}