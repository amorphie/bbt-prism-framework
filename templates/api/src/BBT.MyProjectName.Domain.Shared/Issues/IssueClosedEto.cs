using System;
using BBT.Prism.EventBus;

namespace BBT.MyProjectName.Issues;

[EventName("BBT.MyProjectName.IssueClosed")]
public class IssueClosedEto
{
    public Guid IssueId { get; set; }
    public IssueCloseReason CloseReason { get; set; }
}