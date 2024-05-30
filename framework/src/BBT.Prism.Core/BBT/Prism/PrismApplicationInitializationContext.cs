using System;
using BBT.Prism.DependencyInjection;
using JetBrains.Annotations;

namespace BBT.Prism;

public class PrismApplicationInitializationContext: IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; set; }

    public PrismApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}