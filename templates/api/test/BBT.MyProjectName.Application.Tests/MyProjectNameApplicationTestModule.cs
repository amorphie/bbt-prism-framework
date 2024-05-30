using BBT.Prism.Modularity;

namespace BBT.MyProjectName;

[Modules(
    typeof(MyProjectNameApplicationModule),
    typeof(MyProjectNameDomainTestModule)
)]
public class MyProjectNameApplicationTestModule : PrismModule
{

}
