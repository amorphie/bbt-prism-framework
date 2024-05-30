using System;
using BBT.Prism.EventBus;

namespace BBT.MyProjectName.Issues;

[EventName("BBT.MyProjectName.IssueReOpened")]
public class IssueReOpenedEto
{
    public Guid IssueId { get; set; }
}