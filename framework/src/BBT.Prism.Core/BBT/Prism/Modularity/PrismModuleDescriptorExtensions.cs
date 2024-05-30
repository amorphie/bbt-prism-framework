using System;
using System.Linq;
using System.Reflection;

namespace BBT.Prism.Modularity;

public static class PrismModuleDescriptorExtensions
{
    public static Assembly[] GetAdditionalAssemblies(this IPrismModuleDescriptor module)
    {
        return module.AllAssemblies.Length <= 1
            ? Array.Empty<Assembly>()
            : module.AllAssemblies.Where(x => x != module.Assembly).ToArray();
    }
}