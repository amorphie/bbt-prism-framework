using System;

namespace BBT.Prism.Auditing;

public interface IHasModifyTime
{
    DateTime? LastModificationTime { get; }
}