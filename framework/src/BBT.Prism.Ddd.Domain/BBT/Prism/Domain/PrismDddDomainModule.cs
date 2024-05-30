using BBT.Prism.Auditing;
using BBT.Prism.Data;
using BBT.Prism.EventBus;
using BBT.Prism.ExceptionHandling;
using BBT.Prism.Modularity;
using BBT.Prism.Uow;

namespace BBT.Prism.Domain;

[Modules(
    typeof(PrismAuditingContractsModule),
    typeof(PrismUnitOfWorkModule),
    typeof(PrismDataModule),
    typeof(PrismDddDomainSharedModule),
    typeof(PrismExceptionHandlingModule),
    typeof(PrismEventBusModule)
)]
public class PrismDddDomainModule : PrismModule
{

}