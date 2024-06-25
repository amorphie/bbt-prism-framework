# Value Objects

### Overview

In Domain-Driven Design (DDD), a Value Object is an immutable type that is distinguished only by its properties. Unlike Entities, which have a distinct identity, Value Objects are defined by their attributes and do not have an identity. They are useful for modeling concepts that are fundamentally about values rather than entities.

### Characteristics of Value Objects

1. **Immutability**: Once created, the state of a Value Object cannot be changed. Any modification results in a new Value Object.
2. **Equality**: Two Value Objects are considered equal if all their properties are equal.
3. **Self-Validation**: A Value Object should ensure its own validity by enforcing business rules and invariants in its constructor.

### Example: Implementing a Value Object in .NET Core

Let's create an example of a `Money` Value Object to represent monetary values in different currencies.

#### Step 1: Define the Value Object

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

#### Step 2: Using the Value Object in a Domain Model

Let's use the `Money` Value Object within an `Order` Entity.

```csharp
using System;
using System.Collections.Generic;

    public class Order
    {
        public Guid Id { get; private set; }
        public List<OrderItem> Items { get; private set; }
        public Money TotalAmount { get; private set; }

        public Order()
        {
            Id = Guid.NewGuid();
            Items = new List<OrderItem>();
            TotalAmount = new Money(0, "USD");
        }

        public void AddItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Items.Add(item);
            TotalAmount = TotalAmount.Add(item.Price);
        }
    }

    public class OrderItem
    {
        public Guid Id { get; private set; }
        public string ProductName { get; private set; }
        public Money Price { get; private set; }

        public OrderItem(string productName, Money price)
        {
            Id = Guid.NewGuid();
            ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
            Price = price ?? throw new ArgumentNullException(nameof(price));
        }
    }
```