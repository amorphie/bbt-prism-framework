using System.Threading.Tasks;

namespace BBT.Prism.EventBus.Integrations;

public interface IIntegrationEventHandler<in TEvent> : IEventHandler
{
    /// <summary>
    /// Handler handles the event by implementing this method.
    /// </summary>
    /// <param name="eventData">Event data</param>
    Task HandleEventAsync(TEvent eventData);
}