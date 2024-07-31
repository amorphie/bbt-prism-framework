using System;
using BBT.Prism.DependencyInjection;
using BBT.Prism.Guids;
using BBT.Prism.Linq;
using BBT.Prism.Timing;
using BBT.Prism.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BBT.Prism.Domain.Services;

public abstract class DomainService(IServiceProvider serviceProvider) : IDomainService
{
    public readonly IServiceProvider ServiceProvider = serviceProvider;
    public ILazyServiceProvider LazyServiceProvider => ServiceProvider.GetRequiredService<ILazyServiceProvider>();
    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();
    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();
    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);
}