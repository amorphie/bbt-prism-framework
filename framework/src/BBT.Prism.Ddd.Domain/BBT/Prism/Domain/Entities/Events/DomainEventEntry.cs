using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Events;

[Serializable]
public class DomainEventEntry(object sourceEntity, object eventData, long eventOrder)
{
    public object SourceEntity { get; } = sourceEntity;

    public object EventData { get; } = eventData;

    public long EventOrder { get; } = eventOrder;
}