using System;

namespace BBT.Prism.Auditing;

public interface IModifyAuditedObject : IHasModifyTime
{
    Guid? ModifiedBy { get; }
    Guid? ModifiedByBehalfOf { get;}
}