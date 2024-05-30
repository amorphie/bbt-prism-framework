using System;
using System.Collections.Generic;
using BBT.Prism.Domain.Values;

namespace BBT.MyProjectName.Issues;

public class IssueLabel : ValueObject
{
    public Guid IssueId { get; set; }
    public Guid LabelId { get; set; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return IssueId;
        yield return LabelId;
    }
}