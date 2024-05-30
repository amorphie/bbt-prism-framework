using BBT.Prism;
using BBT.Prism.Modularity;
using BBT.Prism.Testing;

namespace BBT.MyProjectName;

/* All test classes are derived from this class, directly or indirectly. */
public abstract class MyProjectNameTestBase<TStartupModule>: PrismIntegratedTest<TStartupModule>
    where TStartupModule : IPrismModule
{
    protected override void SetPrismApplicationCreationOptions(PrismApplicationCreationOptions options)
    {
    }
}
