using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using BBT.Prism.Logging;
using Microsoft.Extensions.Logging;

namespace BBT.Prism.Validation;

/// <summary>
/// This exception type is used to throws validation exceptions.
/// </summary>
public class PrismValidationException : PrismException,
    IHasLogLevel,
    IHasValidationErrors,
    IExceptionWithSelfLogging
{
    /// <summary>
    /// Detailed list of validation errors for this exception.
    /// </summary>
    public IList<ValidationResult> ValidationErrors { get; }

    /// <summary>
    /// Exception severity.
    /// Default: Warn.
    /// </summary>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    public PrismValidationException()
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    public PrismValidationException(string message)
        : base(message)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    public PrismValidationException(IList<ValidationResult> validationErrors)
    {
        ValidationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="validationErrors">Validation errors</param>
    public PrismValidationException(string message, IList<ValidationResult> validationErrors)
        : base(message)
    {
        ValidationErrors = validationErrors;
        LogLevel = LogLevel.Warning;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public PrismValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
        ValidationErrors = new List<ValidationResult>();
        LogLevel = LogLevel.Warning;
    }

    public void Log(ILogger logger)
    {
        if (ValidationErrors.IsNullOrEmpty())
        {
            return;
        }

        var validationErrors = new StringBuilder();
        validationErrors.AppendLine("There are " + ValidationErrors.Count + " validation errors:");
        foreach (var validationResult in ValidationErrors)
        {
            var memberNames = "";
            if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
            {
                memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
            }

            validationErrors.AppendLine(validationResult.ErrorMessage + memberNames);
        }

        logger.LogWithLevel(LogLevel, validationErrors.ToString());
    }
}
