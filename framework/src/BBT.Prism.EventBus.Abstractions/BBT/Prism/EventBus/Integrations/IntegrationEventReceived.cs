namespace BBT.Prism.EventBus.Integrations;

public class IntegrationEventReceived
{
    public IntegrationEventSource Source { get; set; }

    public string EventName { get; set; } = default!;

    public object EventData { get; set; } = default!;
}