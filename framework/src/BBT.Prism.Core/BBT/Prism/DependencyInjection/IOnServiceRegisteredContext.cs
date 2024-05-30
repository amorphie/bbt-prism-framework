using System;

namespace BBT.Prism.DependencyInjection;

public interface IOnServiceRegisteredContext
{
    Type ImplementationType { get; }
}