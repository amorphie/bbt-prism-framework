using BBT.Prism.Domain;
using BBT.Prism.EntityFrameworkCore.Interceptors;
using BBT.Prism.Linq;
using BBT.Prism.Modularity;
using BBT.Prism.Uow;
using BBT.Prism.Uow.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BBT.Prism.EntityFrameworkCore;

[Modules(typeof(PrismDddDomainModule))]
public class PrismEntityFrameworkCoreModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddSingleton<AuditInterceptor>();
        context.Services.AddSingleton<IAsyncQueryableProvider, EfCoreAsyncQueryableProvider>();
        context.Services.TryAddTransient(typeof(IDbContextProvider<>), typeof(EfCoreDbContextProvider<>));
    }
}
