using System;

namespace BBT.Prism.Data;

public class PrismDbConcurrencyException : PrismException
{
    /// <summary>
    /// Creates a new <see cref="PrismDbConcurrencyException"/> object.
    /// </summary>
    public PrismDbConcurrencyException()
    {

    }

    /// <summary>
    /// Creates a new <see cref="PrismDbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public PrismDbConcurrencyException(string message)
        : base(message)
    {

    }

    /// <summary>
    /// Creates a new <see cref="PrismDbConcurrencyException"/> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public PrismDbConcurrencyException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
