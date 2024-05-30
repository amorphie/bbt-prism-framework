using BBT.Prism.Modularity;

namespace BBT.MyProjectName;

[Modules(
    typeof(MyProjectNameDomainModule),
    typeof(MyProjectNameTestBaseModule)
)]
public class MyProjectNameDomainTestModule: PrismModule
{
    
}