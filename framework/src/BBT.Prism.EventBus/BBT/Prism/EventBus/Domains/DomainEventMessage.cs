using System;

namespace BBT.Prism.EventBus.Domains;

public class DomainEventMessage(Guid messageId, object eventData, Type eventType)
{
    public Guid MessageId { get; } = messageId;

    public object EventData { get; } = eventData;

    public Type EventType { get; } = eventType;
}