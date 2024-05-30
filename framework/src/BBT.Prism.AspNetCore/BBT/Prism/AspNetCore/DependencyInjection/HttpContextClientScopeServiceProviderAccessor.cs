using System;
using BBT.Prism.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace BBT.Prism.AspNetCore.DependencyInjection;

public class HttpContextClientScopeServiceProviderAccessor(IHttpContextAccessor httpContextAccessor) :
    IClientScopeServiceProviderAccessor
{
    public IServiceProvider ServiceProvider {
        get {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new PrismException("HttpContextClientScopeServiceProviderAccessor should only be used in a web request scope!");
            }

            return httpContext.RequestServices;
        }
    }
}
