using BBT.Prism.Collections;

namespace BBT.Prism.Modularity;

public class PrismModuleLifecycleOptions
{
    public ITypeList<IModuleLifecycleContributor> Contributors { get; } = new TypeList<IModuleLifecycleContributor>();
}