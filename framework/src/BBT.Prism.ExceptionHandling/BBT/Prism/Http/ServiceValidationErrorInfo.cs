using System;

namespace BBT.Prism.Http;

/// <summary>
/// Used to store information about a validation error.
/// </summary>
[Serializable]
public class ServiceValidationErrorInfo
{
    /// <summary>
    /// Validation error message.
    /// </summary>
    public string Message { get; set; } = default!;

    /// <summary>
    /// Relate invalid members (fields/properties).
    /// </summary>
    public string[] Members { get; set; } = default!;

    /// <summary>
    /// Creates a new instance of <see cref="ServiceValidationErrorInfo"/>.
    /// </summary>
    public ServiceValidationErrorInfo()
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="ServiceValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    public ServiceValidationErrorInfo(string message)
    {
        Message = message;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ServiceValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    /// <param name="members">Related invalid members</param>
    public ServiceValidationErrorInfo(string message, string[] members)
        : this(message)
    {
        Members = members;
    }

    /// <summary>
    /// Creates a new instance of <see cref="ServiceValidationErrorInfo"/>.
    /// </summary>
    /// <param name="message">Validation error message</param>
    /// <param name="member">Related invalid member</param>
    public ServiceValidationErrorInfo(string message, string member)
        : this(message, new[] { member })
    {

    }
}
