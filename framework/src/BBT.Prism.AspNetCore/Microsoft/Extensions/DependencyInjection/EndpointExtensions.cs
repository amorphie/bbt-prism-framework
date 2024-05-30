using System.Linq;
using BBT.Prism.AspNetCore.Endpoints;
using BBT.Prism.Modularity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints<TType>(this IServiceCollection services) where TType: IPrismModule
    {
        var serviceDescriptors = typeof(TType).Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }
}