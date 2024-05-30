using System.CommandLine;

namespace Commands;

public class CreateCommandArguments : ICommandArguments
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string Type { get; set; } = string.Empty;
}