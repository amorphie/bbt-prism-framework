using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Auditing;

public abstract class CreationAuditedEntity : Entity, ICreationAuditedObject
{
    public virtual DateTime CreatedAt { get; protected set; }
    
    public virtual Guid? CreatedBy { get; protected set; }
    public virtual Guid? CreatedByBehalfOf { get; protected set; }
}

public abstract class CreationAuditedEntity<TKey> : Entity<TKey>, ICreationAuditedObject
{
    public virtual DateTime CreatedAt { get; protected set; }
    
    public virtual Guid? CreatedBy { get; protected set; }
    public virtual Guid? CreatedByBehalfOf { get; protected set; }

    protected CreationAuditedEntity()
    {

    }

    protected CreationAuditedEntity(TKey id)
        : base(id)
    {

    }
}