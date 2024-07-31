using System;

namespace BBT.Prism.Auditing;

public interface IHasDeletionTime : ISoftDelete
{
    DateTime? DeletedAt { get; }
}