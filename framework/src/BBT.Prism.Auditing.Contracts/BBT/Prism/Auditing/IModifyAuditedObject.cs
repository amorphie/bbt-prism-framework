using System;

namespace BBT.Prism.Auditing;

public interface IModifyAuditedObject : IHasModifyTime
{
    Guid? LastModifierId { get; }
}