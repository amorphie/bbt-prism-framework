using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BBT.Prism.Domain.Entites;

namespace BBT.Prism.Domain.Entities;

public abstract class AggregateRoot : BasicAggregateRoot, 
    IHasConcurrencyStamp
{
    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
    
    public virtual string ConcurrencyStamp { get; set; }
}

public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasConcurrencyStamp
{
    public virtual string ConcurrencyStamp { get; set; }

    protected AggregateRoot()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
        ConcurrencyStamp = Guid.NewGuid().ToString("N");
    }
}