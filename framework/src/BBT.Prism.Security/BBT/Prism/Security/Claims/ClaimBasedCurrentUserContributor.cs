using System;
using System.Linq;
using System.Security.Claims;
using BBT.Prism.Users;

namespace BBT.Prism.Security.Claims;

public class ClaimBasedCurrentUserContributor(ClaimsPrincipal claimsPrincipal) : ICurrentUserContributor
{
    public BasicUserInfo? GetCurrentUser()
    {
        if (claimsPrincipal.FindFirst(PrismClaimTypes.UserId)?.Value == null)
        {
            return null;
        }

        return new BasicUserInfo(
            Guid.Parse(claimsPrincipal.FindFirst(PrismClaimTypes.UserId)!.Value),
            claimsPrincipal.FindFirst(PrismClaimTypes.UserName)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Name)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.SurName)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Email)?.Value,
            claimsPrincipal.FindFirst(PrismClaimTypes.Phone)?.Value,
            claimsPrincipal.FindAll(PrismClaimTypes.Role).Select(c => c.Value).ToArray()
        );
    }
}