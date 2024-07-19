using System;

namespace BBT.Prism.Auditing;

public interface IHasCreatedAt
{
    DateTime CreatedAt { get; }
}