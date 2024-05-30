using System.Collections.Generic;

namespace BBT.Prism.Domain.Entities.Events;

public class EntityEventReport
{
    public List<DomainEventEntry> DomainEvents { get; } = new();

    public List<DomainEventEntry> IntegrationEvents { get; } = new();

    public override string ToString()
    {
        return $"[{nameof(EntityEventReport)}] DomainEvents: {DomainEvents.Count}, IntegrationEvents: {IntegrationEvents.Count}";
    } 
}