using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Commands;

internal class Program
{
    private async static Task Main(string[] args)
    {
        var rootCommand = new RootCommand { new CreateCommand() };

        var builder = new CommandLineBuilder(rootCommand).UseDefaults().UseDependencyInjection(services =>
        {
            // Register your services here and use them in your DI-activated command handlers
            // [...]
        });

        await builder.Build().InvokeAsync(args);
    }
}