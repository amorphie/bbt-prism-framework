using System.CommandLine;

namespace Commands;

public class CreateCommand : Command<CreateCommandOptions, CreateCommandHandler, CreateCommandArguments>
{
    // Keep the hard dependency on System.CommandLine here
    //  dotnet run -- create api --name "BBT.Mtls" --output /demo
    public CreateCommand()
        : base("create", "Creates a new project from the template")
    {
        this.AddArgument(new Argument<string>("name", "The name of the new project"));
        this.AddOption(new Option<string>(new []{ "--type", "-t"}, "The type of project to create (e.g., 'api')"));
        this.AddOption(new Option<string>(new []{ "--output", "-o"}, "The output directory for the new project"));
    }
}