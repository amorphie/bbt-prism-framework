using BBT.Prism.Application;
using BBT.Prism.Modularity;

namespace BBT.MyProjectName;

[Modules(
    typeof(MyProjectNameDomainSharedModule),
    typeof(PrismDddApplicationContractsModule)
)]
public class MyProjectNameApplicationContractsModule : PrismModule
{

}