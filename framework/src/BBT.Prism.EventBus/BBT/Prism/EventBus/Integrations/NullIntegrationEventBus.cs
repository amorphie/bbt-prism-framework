using System;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;

namespace BBT.Prism.EventBus.Integrations;

public class NullIntegrationEventBus: IIntegrationEventBus
{
    public static NullIntegrationEventBus Instance { get; } = new NullIntegrationEventBus();

    private NullIntegrationEventBus()
    {

    }

    public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent, THandler>() where TEvent : class where THandler : IEventHandler, new()
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe(Type eventType, IEventHandler handler)
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {
        return NullDisposable.Instance;
    }

    public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
    {
        return NullDisposable.Instance;
    }

    public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
    {

    }

    public void Unsubscribe<TEvent>(IDomainEventHandler<TEvent> handler) where TEvent : class
    {

    }

    public void Unsubscribe(Type eventType, IEventHandler handler)
    {

    }

    public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
    {

    }

    public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
    {

    }

    public void UnsubscribeAll<TEvent>() where TEvent : class
    {

    }

    public void UnsubscribeAll(Type eventType)
    {

    }

    public Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class
    {
        return Task.CompletedTask;
    }

    public Task PublishAsync(Type eventType, object eventData)
    {
        return Task.CompletedTask;
    }
}
