using System;
using System.Collections;

namespace BBT.Prism.Http;

/// <summary>
/// Used to store information about an error.
/// </summary>
[Serializable]
public class ServiceErrorInfo
{
    /// <summary>
    /// Error code.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Error details.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Error data.
    /// </summary>
    public IDictionary? Data { get; set; }

    /// <summary>
    /// Validation errors if exists.
    /// </summary>
    public ServiceValidationErrorInfo[]? ValidationErrors { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="ServiceErrorInfo"/>.
    /// </summary>
    public ServiceErrorInfo()
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="ServiceErrorInfo"/>.
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="details">Error details</param>
    /// <param name="message">Error message</param>
    /// <param name="data">Error data</param>
    public ServiceErrorInfo(string message, string? details = null, string? code = null, IDictionary? data = null)
    {
        Message = message;
        Details = details;
        Code = code;
        Data = data;
    }
}
