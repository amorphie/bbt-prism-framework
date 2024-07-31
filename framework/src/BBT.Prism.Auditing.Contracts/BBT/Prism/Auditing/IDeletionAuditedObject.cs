using System;

namespace BBT.Prism.Auditing;

public interface IDeletionAuditedObject : IHasDeletionTime
{
    Guid? DeletedBy { get; }
}