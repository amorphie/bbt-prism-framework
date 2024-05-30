using System;

namespace BBT.Prism;

public class PrismShutdownException : PrismException
{
    public PrismShutdownException()
    {

    }

    public PrismShutdownException(string message)
        : base(message)
    {

    }

    public PrismShutdownException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
