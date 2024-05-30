using BBT.Prism.Domain;
using BBT.Prism.Mapper;
using BBT.Prism.Modularity;

namespace BBT.Prism.Application;

[Modules(
    typeof(PrismDddDomainModule),
    typeof(PrismDddApplicationContractsModule),
    typeof(PrismMapperModule)
)]
public class PrismDddApplicationModule : PrismModule
{
}