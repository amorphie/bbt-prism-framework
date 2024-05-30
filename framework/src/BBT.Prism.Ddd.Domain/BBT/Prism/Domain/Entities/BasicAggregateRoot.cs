using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BBT.Prism.Uow;

namespace BBT.Prism.Domain.Entities;

[Serializable]
public abstract class BasicAggregateRoot : Entity,
    IAggregateRoot,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _integrationEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _domainEvents = new Collection<DomainEventRecord>();

    public virtual IEnumerable<DomainEventRecord> GetDomainEvents()
    {
        return _domainEvents;
    }
    
    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected virtual void AddDomainEvent(object eventData)
    {
        _domainEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
    
    public virtual IEnumerable<DomainEventRecord> GetIntegrationEvents()
    {
        return _integrationEvents;
    }
    
    public virtual void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    protected virtual void AddIntegrationEvent(object eventData)
    {
        _integrationEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}

[Serializable]
public abstract class BasicAggregateRoot<TKey> : Entity<TKey>,
    IAggregateRoot<TKey>,
    IGeneratesDomainEvents
{
    private readonly ICollection<DomainEventRecord> _integrationEvents = new Collection<DomainEventRecord>();
    private readonly ICollection<DomainEventRecord> _domainEvents = new Collection<DomainEventRecord>();

    protected BasicAggregateRoot()
    {
    }

    protected BasicAggregateRoot(TKey id)
        : base(id)
    {
    }

    public virtual IEnumerable<DomainEventRecord> GetDomainEvents()
    {
        return _domainEvents;
    }

    public virtual void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void AddDomainEvent(object eventData)
    {
        _domainEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
    
    public virtual IEnumerable<DomainEventRecord> GetIntegrationEvents()
    {
        return _integrationEvents;
    }

    public virtual void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    public virtual void AddIntegrationEvent(object eventData)
    {
        _integrationEvents.Add(new DomainEventRecord(eventData, EventOrderGenerator.GetNext()));
    }
}