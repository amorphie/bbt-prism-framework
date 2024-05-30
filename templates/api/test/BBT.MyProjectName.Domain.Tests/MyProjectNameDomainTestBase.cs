using BBT.Prism.Modularity;

namespace BBT.MyProjectName;

/* Inherit from this class for your domain layer tests. */
public abstract class MyProjectNameDomainTestBase<TStartupModule> : MyProjectNameTestBase<TStartupModule>
    where TStartupModule: IPrismModule
{

}
