using System;

namespace BBT.Prism.Auditing;

public interface IHasCreationTime
{
    DateTime CreationTime { get; }
}