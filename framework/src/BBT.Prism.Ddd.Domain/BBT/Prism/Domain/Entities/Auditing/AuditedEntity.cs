using System;
using BBT.Prism.Auditing;

namespace BBT.Prism.Domain.Entities.Auditing;

public abstract class AuditedEntity : CreationAuditedEntity, IAuditedObject
{
    public virtual DateTime? ModifiedAt { get; set; }
    public virtual Guid? ModifiedBy { get; set; }
    public virtual Guid? ModifiedByBehalfOf { get; set; }
}

public abstract class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IAuditedObject
{
    public virtual DateTime? ModifiedAt { get; set; }
    
    public virtual Guid? ModifiedBy { get; set; }
    public virtual Guid? ModifiedByBehalfOf { get; set; }

    protected AuditedEntity()
    {

    }

    protected AuditedEntity(TKey id)
        : base(id)
    {

    }
}