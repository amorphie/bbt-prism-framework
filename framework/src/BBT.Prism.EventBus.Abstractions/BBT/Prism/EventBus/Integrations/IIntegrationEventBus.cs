using System;

namespace BBT.Prism.EventBus.Integrations;

public interface IIntegrationEventBus : IEventBus
{
    /// <summary>
    /// Registers to an event. 
    /// Same (given) instance of the handler is used for all event occurrences.
    /// </summary>
    /// <typeparam name="TEvent">Event type</typeparam>
    /// <param name="handler">Object to handle the event</param>
    IDisposable Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler)
        where TEvent : class;
}