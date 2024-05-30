namespace BBT.Prism.AspNetCore.ExceptionHandling;

public class PrismExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; } = false;

    public bool SendStackTraceToClients { get; set; } = true;
}