using System.CommandLine;

namespace Commands;

public class CreateCommandArguments : ICommandArguments
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string Name { get; set; } = string.Empty;
}