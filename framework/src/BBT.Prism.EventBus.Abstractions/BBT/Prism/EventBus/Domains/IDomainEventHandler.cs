using System.Threading.Tasks;

namespace BBT.Prism.EventBus.Domains;

public interface IDomainEventHandler<in TEvent> : IEventHandler
{
    /// <summary>
    /// Handler handles the event by implementing this method.
    /// </summary>
    /// <param name="eventData">Event data</param>
    Task HandleEventAsync(TEvent eventData);
}