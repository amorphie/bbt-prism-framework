using System;

namespace BBT.Prism;

/// <summary>
/// Base exception type for those are thrown by Prism system for Prism specific exceptions.
/// </summary>
public class PrismException : Exception
{
    public PrismException()
    {

    }

    public PrismException(string? message)
        : base(message)
    {

    }

    public PrismException(string? message, Exception? innerException)
        : base(message, innerException)
    {

    }
}