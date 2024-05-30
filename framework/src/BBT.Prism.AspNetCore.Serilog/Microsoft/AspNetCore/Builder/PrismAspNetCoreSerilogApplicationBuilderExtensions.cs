using BBT.Prism.AspNetCore.Serilog;

namespace Microsoft.AspNetCore.Builder;

public static class PrismAspNetCoreSerilogApplicationBuilderExtensions
{
    public static IApplicationBuilder UsePrismSerilogEnrichers(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<PrismSerilogMiddleware>();
    }
}