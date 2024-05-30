using System;
using System.Threading;
using System.Threading.Tasks;
using BBT.Prism.Application.Services;

namespace BBT.MyProjectName.Issues;

public interface IIssueAppService : IApplicationService
{
    Task<IssueDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IssueDto> CreateAsync(Guid repositoryId, CreateIssueInput input,
        CancellationToken cancellationToken = default);

    Task<IssueDto> UpdateAsync(Guid id, UpdateIssueInput input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task CloseAsync(Guid id, CloseIssueInput input, CancellationToken cancellationToken = default);
    Task ReOpenAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddCommentAsync(Guid id, AddIssueCommentInput input, CancellationToken cancellationToken = default);
}