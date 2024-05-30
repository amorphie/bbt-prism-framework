using System;
using System.Collections.Generic;

namespace BBT.Prism.Reflection;

public interface ITypeFinder
{
    IReadOnlyList<Type> Types { get; }
}