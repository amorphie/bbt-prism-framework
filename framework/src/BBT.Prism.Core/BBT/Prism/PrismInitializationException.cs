using System;

namespace BBT.Prism;

public class PrismInitializationException : PrismException
{
    public PrismInitializationException()
    {

    }

    public PrismInitializationException(string message)
        : base(message)
    {

    }

    public PrismInitializationException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}