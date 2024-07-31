using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BBT.Prism.Users;

public class CurrentUserResolver(
    IOptions<CurrentUserContributorOptions> options,
    IServiceScopeFactory serviceScopeFactory)
    : ICurrentUserResolver
{
    private IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;
    private CurrentUserContributorOptions Options { get; } = options.Value;

    public BasicUserInfo? Resolve()
    {
        using var scope = ServiceScopeFactory.CreateScope();
        foreach (var contributorType in Options.Contributors)
        {
            var contributor = (ICurrentUserContributor)scope.ServiceProvider.GetRequiredService(contributorType);
            var userInfo = contributor.GetCurrentUser();
            if (userInfo != null)
            {
                return userInfo;
            }
        }
        return null;
    }
}