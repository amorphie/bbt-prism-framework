namespace BBT.Prism.EventBus.Integrations;

public class IntegrationEventSent
{
    public IntegrationEventSource Source { get; set; }

    public string EventName { get; set; } = default!;

    public object EventData { get; set; } = default!;
}