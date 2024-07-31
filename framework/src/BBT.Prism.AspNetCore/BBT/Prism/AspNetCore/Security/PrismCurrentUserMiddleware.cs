using System.Threading.Tasks;
using BBT.Prism.Users;
using Microsoft.AspNetCore.Http;

namespace BBT.Prism.AspNetCore.Security;

public class PrismCurrentUserMiddleware(ICurrentUser currentUser, ICurrentUserResolver currentUserResolver)
    : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var basicUserInfo = currentUserResolver.Resolve();
        if (basicUserInfo == null)
        {
            await next(context);
            return;
        }

        using (currentUser.Change(
                   basicUserInfo.Id,
                   basicUserInfo.UserName,
                   basicUserInfo.Name,
                   basicUserInfo.SurName,
                   basicUserInfo.Email,
                   basicUserInfo.Phone,
                   basicUserInfo.Roles))
        {
            await next(context);
        }
    }
}