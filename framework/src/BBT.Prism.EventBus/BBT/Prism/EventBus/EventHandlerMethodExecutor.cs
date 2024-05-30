using System;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;
using BBT.Prism.EventBus.Integrations;

namespace BBT.Prism.EventBus;

public delegate Task EventHandlerMethodExecutorAsync(IEventHandler target, object parameter);

public interface IEventHandlerMethodExecutor
{
    EventHandlerMethodExecutorAsync ExecutorAsync { get; }
}

public class DomainEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
    where TEvent : class
{
    public  EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) => target.As<IDomainEventHandler<TEvent>>().HandleEventAsync(parameter.As<TEvent>());

    public Task ExecuteAsync(IEventHandler target, TEvent parameters)
    {
        return ExecutorAsync(target, parameters);
    }
}

public class IntegrationEventHandlerMethodExecutor<TEvent> : IEventHandlerMethodExecutor
    where TEvent : class
{
    public EventHandlerMethodExecutorAsync ExecutorAsync => (target, parameter) => target.As<IIntegrationEventHandler<TEvent>>().HandleEventAsync(parameter.As<TEvent>());

    public Task ExecuteAsync(IEventHandler target, TEvent parameters)
    {
        return ExecutorAsync(target, parameters);
    }
}
