using System.CommandLine.NamingConventionBinder;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace System.CommandLine;

public abstract class Command<TOptions, THandler, TArguments> : Command
    where TOptions : class, ICommandOptions
    where TArguments: class, ICommandArguments
    where THandler : class, ICommandHandler<TOptions, TArguments>
{
    protected Command(string name, string description)
        : base(name, description)
    {
        this.Handler = CommandHandler.Create<TOptions, TArguments, IServiceProvider, CancellationToken>(HandleOptions);
    }

    private async static Task<int> HandleOptions(TOptions options, TArguments? arguments, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        // True dependency injection happening here
        var handler = ActivatorUtilities.CreateInstance<THandler>(serviceProvider);
        return await handler.HandleAsync(options, arguments, cancellationToken);
    }
}