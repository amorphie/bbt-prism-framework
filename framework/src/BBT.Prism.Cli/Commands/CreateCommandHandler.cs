using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace Commands;

public class CreateCommandHandler(IConsole console) : ICommandHandler<CreateCommandOptions, CreateCommandArguments>
{
    // Inject anything here, no more hard dependency on System.CommandLine
    public Task<int> HandleAsync(CreateCommandOptions options, CreateCommandArguments? arguments, CancellationToken cancellationToken)
    {
        if (arguments != null)
        {
            console.WriteLine($"Arguments: {arguments.Type}");
        }
        console.WriteLine($"Project Name: {options.Name}!");
        console.WriteLine($"Output: {options.Output}!");
        Thread.Sleep(1000);
        return Task.FromResult(0);
    }
}
