using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.EventBus.Integrations;

namespace BBT.Prism.EventBus;

public class EventHandlerInvoker : IEventHandlerInvoker
{
    private readonly ConcurrentDictionary<string, EventHandlerInvokerCacheItem> _cache = new();

    public async Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType)
    {
        var cacheItem = _cache.GetOrAdd($"{eventHandler.GetType().FullName}-{eventType.FullName}", _ =>
        {
            var item = new EventHandlerInvokerCacheItem();

            if (typeof(IDomainEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
            {
                item.Domain = (IEventHandlerMethodExecutor?)Activator.CreateInstance(typeof(DomainEventHandlerMethodExecutor<>).MakeGenericType(eventType));
            }

            if (typeof(IIntegrationEventHandler<>).MakeGenericType(eventType).IsInstanceOfType(eventHandler))
            {
                item.Integration = (IEventHandlerMethodExecutor?)Activator.CreateInstance(typeof(IntegrationEventHandlerMethodExecutor<>).MakeGenericType(eventType));
            }

            return item;
        });

        if (cacheItem.Domain != null)
        {
            await cacheItem.Domain.ExecutorAsync(eventHandler, eventData);
        }

        if (cacheItem.Integration != null)
        {
            await cacheItem.Integration.ExecutorAsync(eventHandler, eventData);
        }

        if (cacheItem.Domain == null && cacheItem.Integration == null)
        {
            throw new PrismException("The object instance is not an event handler. Object type: " + eventHandler.GetType().AssemblyQualifiedName);
        }
    }
}