using System;
using BBT.Prism.DependencyInjection;

namespace BBT.Prism;

public class ApplicationShutdownContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationShutdownContext(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}