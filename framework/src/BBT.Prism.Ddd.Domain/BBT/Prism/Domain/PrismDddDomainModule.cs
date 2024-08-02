using BBT.Prism.Auditing;
using BBT.Prism.Data;
using BBT.Prism.Domain.Services;
using BBT.Prism.EventBus;
using BBT.Prism.ExceptionHandling;
using BBT.Prism.Modularity;
using BBT.Prism.Security.Claims;
using BBT.Prism.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace BBT.Prism.Domain;

[Modules(
    typeof(PrismAuditingContractsModule),
    typeof(PrismUnitOfWorkModule),
    typeof(PrismDataModule),
    typeof(PrismDddDomainSharedModule),
    typeof(PrismExceptionHandlingModule),
    typeof(PrismEventBusModule),
    typeof(PrismSecurityModule)
)]
public class PrismDddDomainModule : PrismModule
{
    public override void ConfigureServices(ModuleConfigurationContext context)
    {
        context.Services.AddTransient<IMultiLingualEntityManager, MultiLingualEntityManager>();
    }
}