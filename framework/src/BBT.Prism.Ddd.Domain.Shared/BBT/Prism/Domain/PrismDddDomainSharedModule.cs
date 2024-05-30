using BBT.Prism.EventBus.Abstractions;
using BBT.Prism.Modularity;

namespace BBT.Prism.Domain;

[Modules(
    typeof(PrismEventBusAbstractionsModule)
    )]
public class PrismDddDomainSharedModule : PrismModule
{
}