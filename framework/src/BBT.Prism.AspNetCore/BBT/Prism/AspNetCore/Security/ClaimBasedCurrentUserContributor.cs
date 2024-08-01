using System;
using System.Linq;
using BBT.Prism.Security.Claims;
using BBT.Prism.Users;
using Microsoft.AspNetCore.Http;

namespace BBT.Prism.AspNetCore.Security;

public class ClaimBasedCurrentUserContributor(IHttpContextAccessor httpContextAccessor) : ICurrentUserContributor
{
    public BasicUserInfo? GetCurrentUser()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        var claimsPrincipal = context.User;

        if (claimsPrincipal.FindFirst(PrismClaimTypes.UserId)?.Value == null)
        {
            return null;
        }

        var actorUserClaim = claimsPrincipal.FindFirst(PrismClaimTypes.Phone)?.Value;
        Guid? actorUserId = null;
        if (actorUserClaim != null)
        {
            if (Guid.TryParse(actorUserClaim, out var parsedActorUserId))
            {
                actorUserId = parsedActorUserId;
            } 
        }

        return new BasicUserInfo(
            Guid.Parse(claimsPrincipal.FindFirst(PrismClaimTypes.UserId)!.Value),
            claimsPrincipal.FindFirst(PrismClaimTypes.UserName)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Name)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.SurName)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Email)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Phone)?.Value,
            claimsPrincipal.FindAll(PrismClaimTypes.Role).Select(c => c.Value).ToArray(),
            actorUserId,
            claimsPrincipal.FindFirst(PrismClaimTypes.ActorSub)?.Value
        );
    }
}