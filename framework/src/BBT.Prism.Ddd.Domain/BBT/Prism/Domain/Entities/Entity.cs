using System.Collections.Generic;

namespace BBT.Prism.Domain.Entities;

public abstract class Entity : IEntity
{
    protected Entity()
    {
        
    }

    public override string ToString()
    {
        return $"{GetType().Name} Keys = {GetKeys().JoinAsString(", ")}";
    }

    public abstract object?[] GetKeys();
}

public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
    public virtual TKey Id { get; protected set; } = default!;

    protected Entity()
    {

    }

    protected Entity(TKey id)
    {
        Id = id;
    }

    public override object?[] GetKeys()
    {
        return new object?[] { Id };
    }
    
    public override string ToString()
    {
        return $"[{GetType().Name}] Id = {Id}";
    }
}