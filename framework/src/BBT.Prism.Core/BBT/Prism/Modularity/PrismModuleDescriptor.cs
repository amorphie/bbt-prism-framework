using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using JetBrains.Annotations;

namespace BBT.Prism.Modularity;

public class PrismModuleDescriptor: IPrismModuleDescriptor
{
    public Type Type { get; }

    public Assembly Assembly { get; }
    
    public Assembly[] AllAssemblies { get; }

    public IPrismModule Instance { get; }

    public bool IsLoadedAsPlugIn { get; }

    public IReadOnlyList<IPrismModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
    private readonly List<IPrismModuleDescriptor> _dependencies;

    public PrismModuleDescriptor(
        [NotNull] Type type,
        [NotNull] IPrismModule instance,
        bool isLoadedAsPlugIn)
    {
        Check.NotNull(type, nameof(type));
        Check.NotNull(instance, nameof(instance));
        PrismModule.CheckPrismModuleType(type);

        if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
        {
            throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
        }

        Type = type;
        Assembly = type.Assembly;
        AllAssemblies = PrismModuleHelper.GetAllAssemblies(type);
        Instance = instance;
        IsLoadedAsPlugIn = isLoadedAsPlugIn;

        _dependencies = new List<IPrismModuleDescriptor>();
    }

    public void AddDependency(IPrismModuleDescriptor descriptor)
    {
        _dependencies.AddIfNotContains(descriptor);
    }

    public override string ToString()
    {
        return $"[PrismModuleDescriptor {Type.FullName}]";
    }
}
