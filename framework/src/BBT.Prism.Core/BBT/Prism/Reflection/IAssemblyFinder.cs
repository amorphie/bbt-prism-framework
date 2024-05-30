using System.Collections.Generic;
using System.Reflection;

namespace BBT.Prism.Reflection;

public interface IAssemblyFinder
{
    IReadOnlyList<Assembly> Assemblies { get; }
}