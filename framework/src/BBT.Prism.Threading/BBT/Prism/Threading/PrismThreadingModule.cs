using BBT.Prism.Linq;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Threading;

public class PrismThreadingModule: PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<IAsyncQueryableExecuter, AsyncQueryableExecuter>();
    }
}