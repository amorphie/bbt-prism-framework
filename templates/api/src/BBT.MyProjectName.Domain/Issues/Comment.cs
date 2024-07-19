using System;
using System.Threading.Tasks;
using BBT.Prism.Auditing;
using BBT.Prism.Domain.Entities;

namespace BBT.MyProjectName.Issues;

public class Comment: Entity<Guid>, IHasCreatedAt
{
    public string Text { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Guid IssueId { get; private set; }
    public Guid UserId { get; private set; }

    internal Comment(
        Guid id,
        string text,
        Guid issueId,
        Guid userId): base(id)
    {
        Text = text;
        Text = text;
        IssueId = issueId;
        UserId = userId;
    }
}