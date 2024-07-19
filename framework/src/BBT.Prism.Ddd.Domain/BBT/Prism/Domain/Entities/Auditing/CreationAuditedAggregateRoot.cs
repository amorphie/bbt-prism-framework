using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Auditing;

public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
    public virtual DateTime CreatedAt { get; protected set; }
    public virtual Guid? CreatedBy { get; protected set; }
    public virtual Guid? CreatedByBehalfOf { get; protected set; }
}

public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
    public virtual DateTime CreatedAt { get; set; }
    public virtual Guid? CreatedBy { get; set; }
    public virtual Guid? CreatedByBehalfOf { get; set; }

    protected CreationAuditedAggregateRoot()
    {

    }

    protected CreationAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}