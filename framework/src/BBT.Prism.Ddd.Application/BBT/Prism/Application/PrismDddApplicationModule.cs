using BBT.Prism.Domain;
using BBT.Prism.Mapper;
using BBT.Prism.Modularity;
using BBT.Prism.Security.Claims;

namespace BBT.Prism.Application;

[Modules(
    typeof(PrismDddDomainModule),
    typeof(PrismDddApplicationContractsModule),
    typeof(PrismMapperModule),
    typeof(PrismSecurityModule)
)]
public class PrismDddApplicationModule : PrismModule
{
}