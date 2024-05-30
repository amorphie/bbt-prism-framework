using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BBT.Prism;
using BBT.Prism.Domain.Entities.Auditing;

namespace BBT.MyProjectName.Issues;

public class Issue : AuditedAggregateRoot<Guid>
{
    public Guid RepositoryId { get; private set; }
    public string Title { get; private set; }
    public string? Text { get; set; }
    public bool IsLocked { get; private set; }
    public bool IsClosed { get; private set; }
    public IssueCloseReason? CloseReason { get; private set; }
    public Guid? AssignedUserId { get; private set; }
    public DateTime? LastCommentTime { get; private set; }
    public virtual ICollection<IssueLabel> Labels { get; private set; }
    public virtual ICollection<Comment> Comments { get; private set; }

    public void SetTitle(string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title));
    }

    public void Close(IssueCloseReason reason)
    {
        IsClosed = true;
        CloseReason = reason;
        AddIntegrationEvent(new IssueClosedEto { IssueId = Id, CloseReason = reason });
    }

    public void ReOpen()
    {
        if (IsLocked)
        {
            throw new IssueStateException("Can not open a locked issue! Unlock it first.");
        }

        IsClosed = false;
        CloseReason = null;

        AddDomainEvent(new IssueReOpenedEto { IssueId = Id });
    }

    public void Lock()
    {
        if (!IsClosed)
        {
            throw new IssueStateException("Can not open a locked issue! Unlock it first.");
        }

        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }

    public void AddComment(string text, Guid userId)
    {
        Comments.Add(new Comment(Guid.NewGuid(), text, Id, userId));
    }

    public Issue(
        Guid id,
        Guid repositoryId,
        string title,
        string? text = null,
        Guid? assignedUserId = null
    ) : base(id)

    {
        RepositoryId = repositoryId;
        SetTitle(title);
        Text = text;
        AssignedUserId = assignedUserId;

        Labels = new Collection<IssueLabel>();
        Comments = new Collection<Comment>();
    }

    private Issue()
    {
        //for orm
    }
}