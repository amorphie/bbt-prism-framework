using System;
using System.Collections.Generic;
using BBT.Prism.AspNetCore.DependencyInjection;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Prism.AspNetCore.ExceptionHandling;
using BBT.Prism.AspNetCore.Security;
using BBT.Prism.AspNetCore.Tracing;
using BBT.Prism.Auditing;
using BBT.Prism.DependencyInjection;
using BBT.Prism.ExceptionHandling;
using BBT.Prism.Modularity;
using BBT.Prism.Security.Claims;
using BBT.Prism.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.AspNetCore;

[Modules(
    typeof(PrismAuditingContractsModule),
    typeof(PrismExceptionHandlingModule),
    typeof(PrismUnitOfWorkModule),
    typeof(PrismSecurityModule)
)]
public class PrismAspNetCoreModule : PrismModule
{
    public override void PreConfigureServices(ModuleConfigurationContext context)
    {
        var hostEnvironment = context.Services.GetSingletonInstance<IPrismHostEnvironment>();
        if (hostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
        {
            hostEnvironment.EnvironmentName = context.Services.GetHostingEnvironment().EnvironmentName;
        }
    }

    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        AddAspNetServices(context.Services);

        context.Services.AddTransient<ClaimBasedCurrentUserContributor>();
        context.Services.AddTransient<HeaderCurrentUserContributor>();
        
        //Exception
        context.Services.AddTransient<IHttpExceptionStatusCodeFinder, DefaultHttpExceptionStatusCodeFinder>();
        
        //Middleware
        context.Services.AddTransient<PrismCurrentUserMiddleware>();
        context.Services.AddTransient<PrismCorrelationIdMiddleware>();
        context.Services.AddTransient<PrismSecurityHeadersMiddleware>();
        context.Services.AddExceptionHandler();
        
        //Application
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        
        // NOTE: Moved to PrismApplicationBuilderExtensions.MapEndpoints extension. Asp.Versioning.Http does not support IApplicationBuilder yet. 
        // Configure<PrismEndpointRouterOptions>(options =>
        // {
        //     options.EndpointConfigureActions.Add(builderContext =>
        //     {
        //         var endpoints = builderContext.ScopeServiceProvider.GetRequiredService<IEnumerable<IEndpoint>>();
        //         foreach (var endpoint in endpoints)
        //         {
        //             endpoint.MapEndpoint(builderContext.Endpoints);
        //         }
        //     });
        // });
    }
    
    private static void AddAspNetServices(IServiceCollection services)
    {
        services.AddSingleton<IClientScopeServiceProviderAccessor, HttpContextClientScopeServiceProviderAccessor>();
        services.AddHttpContextAccessor();
    }
}