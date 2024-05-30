using System.Threading.Tasks;
using BBT.Prism.EventBus.Domains;
using Microsoft.Extensions.Logging;

namespace BBT.MyProjectName.Issues.Handlers;

public class IssueReOpenedEventHandler(ILogger<IssueClosedEventHandler> logger) : IDomainEventHandler<IssueReOpenedEto>
{
    public Task HandleEventAsync(IssueReOpenedEto eventData)
    {
        logger.LogInformation($"{nameof(IssueClosedEto)}: IssueId: {eventData.IssueId}");
        return Task.CompletedTask;
    }
}