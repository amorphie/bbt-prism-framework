using System;
using System.Threading.Tasks;

namespace BBT.Prism.EventBus;

public interface IEventHandlerInvoker
{
    Task InvokeAsync(IEventHandler eventHandler, object eventData, Type eventType);
}
