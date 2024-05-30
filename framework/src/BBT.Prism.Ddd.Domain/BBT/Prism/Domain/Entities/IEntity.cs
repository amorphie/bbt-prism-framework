namespace BBT.Prism.Domain.Entities;

public interface IEntity
{
    object?[] GetKeys();
}

public interface IEntity<TKey> : IEntity
{
    /// <summary>
    /// Unique id
    /// </summary>
    TKey Id { get; }
}