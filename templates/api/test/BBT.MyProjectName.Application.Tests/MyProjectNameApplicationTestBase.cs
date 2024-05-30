using BBT.Prism.Modularity;

namespace BBT.MyProjectName;

public abstract class MyProjectNameApplicationTestBase<TStartupModule>: MyProjectNameTestBase<TStartupModule>
    where TStartupModule: IPrismModule
{
}