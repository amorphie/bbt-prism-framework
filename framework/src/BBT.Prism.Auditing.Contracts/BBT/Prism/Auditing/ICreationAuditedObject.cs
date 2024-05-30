using System;

namespace BBT.Prism.Auditing;

public interface ICreationAuditedObject : IHasCreationTime
{
    Guid? CreatorId { get; }
}