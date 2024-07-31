using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Auditing;

public abstract class FullAuditedAggregateRoot: AuditedAggregateRoot, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }
    public virtual Guid? DeletedBy { get; set; }
    public virtual DateTime? DeletedAt { get; set; }
}

public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject
{
    public virtual bool IsDeleted { get; set; }
    public virtual Guid? DeletedBy { get; set; }
    public virtual DateTime? DeletedAt { get; set; }

    protected FullAuditedAggregateRoot()
    {

    }

    protected FullAuditedAggregateRoot(TKey id)
        : base(id)
    {

    }
}