using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceDescriptorExtensions
{
    public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor)
    {
        return descriptor.IsKeyedService 
            ? descriptor.KeyedImplementationInstance 
            : descriptor.ImplementationInstance;
    }

    public static Type? NormalizedImplementationType(this ServiceDescriptor descriptor)
    {
        return descriptor.IsKeyedService
            ? descriptor.KeyedImplementationType
            : descriptor.ImplementationType;
    }
}