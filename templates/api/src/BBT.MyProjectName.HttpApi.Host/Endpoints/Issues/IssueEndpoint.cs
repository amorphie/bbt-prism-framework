using System;
using System.Threading;
using BBT.MyProjectName.Issues;
using BBT.Prism.AspNetCore.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BBT.MyProjectName.Endpoints.Issues;

public class IssueEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("issues")
            .WithTags("Issues")
            .MapToApiVersion(1);

        group.MapGet("/{id}", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await issueAppService.GetAsync(id, cancellationToken)
        );

        group.MapPost("/{repositoryId}", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid repositoryId,
                [FromBody] CreateIssueInput input,
                CancellationToken cancellationToken
            ) => await issueAppService.CreateAsync(repositoryId, input, cancellationToken)
        );

        group.MapPut("/{id}", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                [FromBody] UpdateIssueInput input,
                CancellationToken cancellationToken
            ) => await issueAppService.UpdateAsync(id, input, cancellationToken)
        );

        group.MapDelete("/{id}", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await issueAppService.DeleteAsync(id, cancellationToken)
        );

        group.MapPost("/{id}/close", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                [FromBody] CloseIssueInput input,
                CancellationToken cancellationToken
            ) => await issueAppService.CloseAsync(id, input, cancellationToken)
        );

        group.MapPost("/{id}/reopen", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                CancellationToken cancellationToken
            ) => await issueAppService.ReOpenAsync(id, cancellationToken)
        );

        group.MapPost("/{id}/comment", async (
                [FromServices] IIssueAppService issueAppService,
                [FromRoute] Guid id,
                [FromBody] AddIssueCommentInput input,
                CancellationToken cancellationToken
            ) => await issueAppService.AddCommentAsync(id, input, cancellationToken)
        );
    }
}