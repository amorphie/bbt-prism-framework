using System;

namespace Microsoft.AspNetCore.Routing;

public class EndpointRouteBuilderContext(
    IEndpointRouteBuilder endpoints,
    IServiceProvider scopeServiceProvider)
{
    public IEndpointRouteBuilder Endpoints { get; } = endpoints;

    public IServiceProvider ScopeServiceProvider { get; } = scopeServiceProvider;
}