using System.Threading.Tasks;
using BBT.Prism.EventBus.Integrations;
using Microsoft.Extensions.Logging;

namespace BBT.MyProjectName.Issues.Handlers;

public class IssueClosedEventHandler(ILogger<IssueClosedEventHandler> logger) : IIntegrationEventHandler<IssueClosedEto>
{
    public Task HandleEventAsync(IssueClosedEto eventData)
    {
        logger.LogInformation($"{nameof(IssueClosedEto)}: IssueId: {eventData.IssueId}");
        return Task.CompletedTask;
    }
}