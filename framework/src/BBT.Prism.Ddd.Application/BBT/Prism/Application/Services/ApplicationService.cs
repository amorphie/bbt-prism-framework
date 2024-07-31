using System;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Guids;
using BBT.Prism.Linq;
using BBT.Prism.Mapper;
using BBT.Prism.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BBT.Prism.Application.Services;

public abstract class ApplicationService(IServiceProvider serviceProvider)
    : IApplicationService, IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; } = serviceProvider;

    public ILazyServiceProvider LazyServiceProvider { get; } =
        serviceProvider.GetRequiredService<ILazyServiceProvider>();
    
    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetRequiredService<IGuidGenerator>();
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
    protected ILogger Logger => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance;
}