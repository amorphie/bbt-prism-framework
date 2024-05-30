using Microsoft.AspNetCore.Routing;

namespace BBT.Prism.AspNetCore.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}