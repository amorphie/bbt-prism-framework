using System;
using System.Linq;
using System.Reflection;

namespace BBT.Prism.Modularity;

/// <summary>
/// Used to define additional assemblies for a module.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AdditionalAssemblyAttribute(params Type[]? typesInAssemblies)
    : Attribute, IAdditionalModuleAssemblyProvider
{
    public Type[] TypesInAssemblies { get; } = typesInAssemblies ?? Type.EmptyTypes;

    public virtual Assembly[] GetAssemblies()
    {
        return TypesInAssemblies.Select(t => t.Assembly).Distinct().ToArray();
    }
}