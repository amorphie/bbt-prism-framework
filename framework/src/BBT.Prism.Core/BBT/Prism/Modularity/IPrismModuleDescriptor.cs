using System;
using System.Collections.Generic;
using System.Reflection;

namespace BBT.Prism.Modularity;

public interface IPrismModuleDescriptor
{
    /// <summary>
    /// Type of the module class.
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// Main assembly that defines the module <see cref="Type"/>.
    /// </summary>
    Assembly Assembly { get; }
    
    /// <summary>
    /// All the assemblies of the module.
    /// Includes the main <see cref="Assembly"/> and other assemblies defined
    /// on the module <see cref="Type"/> using the <see cref="AdditionalAssemblyAttribute"/> attribute.
    /// </summary>
    Assembly[] AllAssemblies { get; }

    /// <summary>
    /// The instance of the module class (singleton).
    /// </summary>
    IPrismModule Instance { get; }

    /// <summary>
    /// Modules on which this module depends on.
    /// A module can depend on another module using the <see cref="ModulesAttribute"/> attribute.
    /// </summary>
    IReadOnlyList<IPrismModuleDescriptor> Dependencies { get; }
}