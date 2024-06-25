# Domain Service

# Overview

In Domain-Driven Design (DDD), a Domain Service is a service that contains domain logic which doesn't naturally fit within an Entity or a Value Object. Domain Services are used to encapsulate business logic that involves multiple entities or requires coordination between them.

## Characteristics of Domain Services

1. **Statelessness**: Domain Services typically do not maintain any state. They operate on the state of entities or value objects.
2. **Domain Logic**: They contain business logic that cannot be naturally placed within an entity or value object.
3. **Coordination**: They often coordinate operations across multiple entities.

### Example: Implementing a Domain Service in .NET Core

Either derive a Domain Service from the `DomainService` base class or directly implement the `IDomainService` interface.

Let's create an example of a `TransferService` Domain Service to handle money transfers between accounts.

#### Step 1: Define the Entities

First, define the `Account` entity.

```csharp
using System;

public class Account
{
    public Guid Id { get; private set; }
    public string Owner { get; private set; }
    public Money Balance { get; private set; }

    public Account(Guid id, string owner, Money initialBalance)
    {
        Id = id;
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        Balance = initialBalance ?? throw new ArgumentNullException(nameof(initialBalance));
    }

    public void Deposit(Money amount)
    {
        Balance = Balance.Add(amount);
    }

    public void Withdraw(Money amount)
    {
        if (Balance.Amount < amount.Amount)
            throw new InvalidOperationException("Insufficient funds");

        Balance = new Money(Balance.Amount - amount.Amount, Balance.Currency);
    }
}
```

#### Step 2: Define the Value Object

The `Money` Value Object remains the same as defined earlier.

```csharp
using System;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency;
    }
}
```

#### Step 3: Define the Domain Service

Next, define the `TransferService` Domain Service.

```csharp
using System;

public class TransferService: DomainService
{
    public void Transfer(Account fromAccount, Account toAccount, Money amount)
    {
        if (fromAccount == null)
            throw new ArgumentNullException(nameof(fromAccount));
        if (toAccount == null)
            throw new ArgumentNullException(nameof(toAccount));
        if (amount == null)
            throw new ArgumentNullException(nameof(amount));

        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);
    }
}
```