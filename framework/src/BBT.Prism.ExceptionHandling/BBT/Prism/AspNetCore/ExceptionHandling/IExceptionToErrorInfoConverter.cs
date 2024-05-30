using System;
using BBT.Prism.Http;

namespace BBT.Prism.AspNetCore.ExceptionHandling;

/// <summary>
/// This interface can be implemented to convert an <see cref="Exception"/> object to an <see cref="ServiceErrorInfo"/> object.
/// Implements Chain Of Responsibility pattern.
/// </summary>
public interface IExceptionToErrorInfoConverter
{
    /// <summary>
    /// Converter method.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <param name="options">Additional options.</param>
    /// <returns>Error info or null</returns>
    ServiceErrorInfo Convert(Exception exception, Action<PrismExceptionHandlingOptions>? options = null);
}
