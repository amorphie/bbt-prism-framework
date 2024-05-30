using System.Threading;
using System.Threading.Tasks;

namespace System.CommandLine;

public interface ICommandHandler<in TOptions, in TArguments>
{
    Task<int> HandleAsync(TOptions options, TArguments? arguments, CancellationToken cancellationToken);
}


