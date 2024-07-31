using System;
using System.Collections.Generic;
using BBT.Prism.Modularity;
using BBT.Prism.Users;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Security.Claims;

public class PrismSecurityModule : PrismModule
{
    public override void PreConfigureServices(ModuleConfigurationContext context)
    {
        var contributors = new List<Type>();
        context.Services.OnRegistered(registerContext =>
        {
            if (typeof(ICurrentUserContributor).IsAssignableFrom(registerContext.ImplementationType))
            {
                contributors.Add(registerContext.ImplementationType);
            }
        });
        
        context.Services.Configure<CurrentUserContributorOptions>(options =>
        {
            options.Contributors.AddIfNotContains(contributors);
        });
    }

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<ClaimBasedCurrentUserContributor>();
        context.Services.AddTransient<ICurrentUserAccessor, AsyncLocalCurrentUserAccessor>();
        context.Services.AddTransient<ICurrentUserResolver, CurrentUserResolver>();
        context.Services.AddTransient<ICurrentUser, CurrentUser>();
    }
}