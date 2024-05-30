using BBT.Prism.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection;

public static class PrismAspNetCoreServiceCollectionExtensions
{
    public static IWebHostEnvironment GetHostingEnvironment(this IServiceCollection services)
    {
        var hostingEnvironment = services.GetSingletonInstanceOrNull<IWebHostEnvironment>();

        if (hostingEnvironment == null)
        {
            return new EmptyHostingEnvironment()
            {
                EnvironmentName = Environments.Development
            };
        }

        return hostingEnvironment;
    }
    
    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<PrismExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
}