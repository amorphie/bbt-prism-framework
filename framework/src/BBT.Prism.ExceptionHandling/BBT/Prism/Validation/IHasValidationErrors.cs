using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BBT.Prism.Validation;

public interface IHasValidationErrors
{
    IList<ValidationResult> ValidationErrors { get; }
}