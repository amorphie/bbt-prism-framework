using System;
using System.Linq;
using BBT.Prism.Security.Claims;
using BBT.Prism.Users;
using Microsoft.AspNetCore.Http;

namespace BBT.Prism.AspNetCore.Security;

public class HeaderCurrentUserContributor(IHttpContextAccessor httpContextAccessor) : ICurrentUserContributor
{
    public BasicUserInfo? GetCurrentUser()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        var userId = context.Request.Headers[PrismClaimTypes.UserId];
        if (Guid.TryParse(userId, out var parsedUserId))
        {
            return null;
        }

        var userName = context.Request.Headers[PrismClaimTypes.UserName].FirstOrDefault() ?? string.Empty;
        var name = context.Request.Headers[PrismClaimTypes.Name].FirstOrDefault() ?? string.Empty;
        var surname = context.Request.Headers[PrismClaimTypes.SurName].FirstOrDefault() ?? string.Empty;
        var email = context.Request.Headers[PrismClaimTypes.Email].FirstOrDefault() ?? string.Empty;
        var phone = context.Request.Headers[PrismClaimTypes.Phone].FirstOrDefault() ?? string.Empty;
        var rolesHeader = context.Request.Headers[PrismClaimTypes.Role].FirstOrDefault();
        var roles = rolesHeader != null ? rolesHeader.Split(',') : [];

        return new BasicUserInfo(
            parsedUserId,
            userName,
            name,
            surname,
            email,
            phone,
            roles
        );
    }
}