using BBT.Prism.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace BBT.Prism.AspNetCore.Microsoft.AspNetCore.Cors;

public static class PrismCorsPolicyBuilderExtensions
{
    public static CorsPolicyBuilder WithPrismExposedHeaders(this CorsPolicyBuilder corsPolicyBuilder)
    {
        return corsPolicyBuilder.WithExposedHeaders(PrismExceptionHandler.ErrorFormat);
    }
}
