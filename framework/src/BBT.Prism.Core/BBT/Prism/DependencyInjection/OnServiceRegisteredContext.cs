using System;

namespace BBT.Prism.DependencyInjection;

public sealed class OnServiceRegisteredContext(Type serviceType, Type implementationType) : IOnServiceRegisteredContext
{
    public Type ServiceType { get; } = Check.NotNull(serviceType, nameof(serviceType));
    public Type ImplementationType { get; } = Check.NotNull(implementationType, nameof(implementationType));
}
