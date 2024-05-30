using System;

namespace BBT.Prism.EventBus;

public class EventHandlerDisposeWrapper(IEventHandler eventHandler, Action? disposeAction = null)
    : IEventHandlerDisposeWrapper
{
    public IEventHandler EventHandler { get; } = eventHandler;

    public void Dispose()
    {
        disposeAction?.Invoke();
    }
}
