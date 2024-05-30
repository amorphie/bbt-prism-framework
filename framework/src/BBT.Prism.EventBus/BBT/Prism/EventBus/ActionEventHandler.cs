using System;
using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;

namespace BBT.Prism.EventBus;

/// <summary>
/// This event handler is an adapter to be able to use an action as <see cref="IDomainEventHandler{TEvent}"/> implementation.
/// </summary>
/// <typeparam name="TEvent">Event type</typeparam>
public class ActionEventHandler<TEvent> :
    IDomainEventHandler<TEvent>
    //ITransientDependency
{
    /// <summary>
    /// Function to handle the event.
    /// </summary>
    public Func<TEvent, Task> Action { get; }

    /// <summary>
    /// Creates a new instance of <see cref="ActionEventHandler{TEvent}"/>.
    /// </summary>
    /// <param name="handler">Action to handle the event</param>
    public ActionEventHandler(Func<TEvent, Task> handler)
    {
        Action = handler;
    }

    /// <summary>
    /// Handles the event.
    /// </summary>
    /// <param name="eventData"></param>
    public async Task HandleEventAsync(TEvent eventData)
    {
        await Action(eventData);
    }
}
