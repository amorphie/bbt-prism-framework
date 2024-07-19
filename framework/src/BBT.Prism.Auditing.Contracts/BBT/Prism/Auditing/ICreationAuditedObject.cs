using System;

namespace BBT.Prism.Auditing;

public interface ICreationAuditedObject : IHasCreatedAt
{
    Guid? CreatedBy { get; }
    Guid? CreatedByBehalfOf { get; }
}