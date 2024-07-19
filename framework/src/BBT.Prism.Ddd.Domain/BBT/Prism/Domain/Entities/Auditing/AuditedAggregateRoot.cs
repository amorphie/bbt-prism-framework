using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Auditing;

public abstract class AuditedAggregateRoot: CreationAuditedAggregateRoot, IAuditedObject
{
    public virtual DateTime? ModifiedAt { get; set; }
    
    public virtual Guid? ModifiedBy { get; set; }
    public virtual Guid? ModifiedByBehalfOf { get; set;}
}

public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAuditedObject
{
    public virtual DateTime? ModifiedAt { get; set; }
    public virtual Guid? ModifiedBy { get; set; }
    public virtual Guid? ModifiedByBehalfOf { get; set;}

    protected AuditedAggregateRoot()
    {

    }

    protected AuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}