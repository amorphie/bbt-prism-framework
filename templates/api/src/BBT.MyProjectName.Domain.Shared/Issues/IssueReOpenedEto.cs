using System;
using BBT.Prism.EventBus;

namespace BBT.MyProjectName.Issues;

[EventName("BBT.MyNameProject.IssueReOpened")]
public class IssueReOpenedEto
{
    public Guid IssueId { get; set; }
}