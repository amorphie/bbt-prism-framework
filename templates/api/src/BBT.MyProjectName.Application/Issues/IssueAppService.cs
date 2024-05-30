using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;
using BBT.Prism.Uow;

namespace BBT.MyProjectName.Issues;

public class IssueAppService(
    IServiceProvider serviceProvider,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork) 
    : ApplicationService(serviceProvider), IIssueAppService
{
    
    public async Task<IssueDto> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var issue = await issueRepository.GetAsync(id, true, cancellationToken);
        return ObjectMapper.Map<Issue, IssueDto>(issue);
    }

    public async Task<IssueDto> CreateAsync(Guid repositoryId, CreateIssueInput input, CancellationToken cancellationToken = default)
    {
        var issue = new Issue(
            GuidGenerator.Create(),
            repositoryId,
            input.Title,
            input.Text
        );

        await issueRepository.InsertAsync(issue, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Issue, IssueDto>(issue);
    }

    public async Task<IssueDto> UpdateAsync(Guid id, UpdateIssueInput input, CancellationToken cancellationToken = default)
    {
        var issue = await issueRepository.GetAsync(id, true, cancellationToken);
        issue.SetTitle(input.Title);
        issue.Text = input.Text;
        await issueRepository.UpdateAsync(issue, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return ObjectMapper.Map<Issue, IssueDto>(issue);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await issueRepository.DeleteAsync(id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CloseAsync(Guid id, CloseIssueInput input, CancellationToken cancellationToken = default)
    {
        var issue = await issueRepository.GetAsync(id, true, cancellationToken);
        issue.Close(input.CloseReason);
        await issueRepository.UpdateAsync(issue, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReOpenAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var issue = await issueRepository.GetAsync(id, true, cancellationToken);
        issue.ReOpen();
        await issueRepository.UpdateAsync(issue, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task AddCommentAsync(Guid id, AddIssueCommentInput input, CancellationToken cancellationToken = default)
    {
        var issue = await issueRepository.GetAsync(id, true, cancellationToken);
        issue.AddComment(input.Text, Guid.NewGuid());
        issue.ConcurrencyStamp = input.ConcurrencyStamp;
        await issueRepository.UpdateAsync(issue, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}