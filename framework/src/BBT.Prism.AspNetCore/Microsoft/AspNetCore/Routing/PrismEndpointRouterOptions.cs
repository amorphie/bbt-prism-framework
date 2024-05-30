using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing;

public class PrismEndpointRouterOptions
{
    public List<Action<EndpointRouteBuilderContext>> EndpointConfigureActions { get; } = new();
}