using System.Threading.Tasks;
using BBT.Prism;
using BBT.Prism.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class PrismHostExtensions
{
    public async static Task InitializeAsync(this IHost host)
    {
        var application = host.Services.GetRequiredService<IApplicationServiceProvider>();
        var applicationLifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

        applicationLifetime.ApplicationStopping.Register(() => AsyncHelper.RunSync(() => application.ShutdownAsync()));
        applicationLifetime.ApplicationStopped.Register(() => application.Dispose());

        await application.InitializeAsync(host.Services);
    }
}