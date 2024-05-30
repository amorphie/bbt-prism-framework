using System;

namespace BBT.Prism.EventBus;


/// <summary>
/// Used to unregister a <see cref="IEventHandlerFactory"/> on <see cref="Dispose"/> method.
/// </summary>
public class EventHandlerFactoryUnregistrar(IEventBus eventBus, Type eventType, IEventHandlerFactory factory)
    : IDisposable
{
    public void Dispose()
    {
        eventBus.Unsubscribe(eventType, factory);
    }
}
