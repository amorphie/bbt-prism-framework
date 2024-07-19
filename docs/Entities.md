# Entites

An entity is generally mapped to a table in a relational database.

# Entity Class

Entities are derived from the ``Entity<TKey>`` class as shown below:

```charp
public Issue : Entity<Guid>
{
    ....
}
```

``Entity<TKey>`` class just defines an Id property with the given primary key type, which is ``Guid`` in the example above. It can be other types like ``string``, ``int``, ``long``, or whatever you need.

## Entities with Composite Keys

Some entities may need to have composite keys. In that case, you can derive your entity from the non-generic ``Entity`` class. Example:

```charp
public IssueRole : Entity
{
    public Guid IssueId {get; set;}
    public Guid RoleId {get; set;}

    public ovveride object[] GetKeys()
    {
        return new object[] { IssueId, RoleId };
    }
    ....
}
```
For the example above, the composite key is composed of ``IssueId`` and ``RoleId``. For a relational database, it is the composite primary key of the related table. Entities with composite keys should implement the ``GetKeys()`` method as shown above.

## Auditing Interfaces
There are a lot of auditing interfaces, so you can implement the one that you need.


``IHasCreatedAt`` defines the following properties:
    ``CreatedAt``

``ICreationAuditedObject``  defines the following properties:
    ``CreatedAt``
    ``CreatedBy``

``IHasModifyTime`` defines the following properties:
    ``ModifiedAt``

``IModifyAuditedObject`` defines the following properties:
    ``ModifiedAt``
    ``ModifiedBy``

``IAuditedObject`` defines the following properties:
    ``CreatedAt``
    ``CreatedBy``
    ``ModifiedAt``
    ``ModifiedBy``

``ISoftDelete`` defines the following properties:
    ``IsDeleted``

``IHasDeletionTime`` defines the following properties:
    ``IsDeleted``
    ``DeletionTime``

``IDeletionAuditedObject`` defines the following properties:
    ``IsDeleted``
    ``DeletionTime``
    ``DeleterId``

``IFullAuditedObject`` defines the following properties:
    ``CreatedAt``
    ``CreatedBy``
    ``ModifiedAt``
    ``ModifiedBy``
    ``IsDeleted``
    ``DeletionTime``
    ``DeleterId``

## Auditing Base Classes
While you can manually implement any of the interfaces defined above, it is suggested to inherit from the base classes defined here:

* ``CreationAuditedEntity<TKey>`` and ``CreationAuditedAggregateRoot<TKey>`` implement the ``ICreationAuditedObject`` interface.
* ``AuditedEntity<TKey>`` and ``AuditedAggregateRoot<TKey>`` implement the ``IAuditedObject`` interface.
* ``FullAuditedEntity<TKey>`` and ``FullAuditedAggregateRoot<TKey>`` implement the ``IFullAuditedObject`` interface.

All these base classes also have non-generic versions to take ``AuditedEntity`` and ``ullAuditedAggregateRoot`` to support the composite primary keys.