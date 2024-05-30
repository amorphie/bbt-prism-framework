namespace BBT.Prism.EventBus.Dapr;

public class PrismDaprEventData(
    string pubSubName,
    string topic,
    string messageId,
    string jsonData,
    string? correlationId)
{
    public string PubSubName { get; set; } = pubSubName;

    public string Topic { get; set; } = topic;

    public string MessageId { get; set; } = messageId;

    public string JsonData { get; set; } = jsonData;

    public string? CorrelationId { get; set; } = correlationId;
}