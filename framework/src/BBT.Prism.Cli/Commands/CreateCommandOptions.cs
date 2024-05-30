using System.CommandLine;

namespace Commands;

public class CreateCommandOptions : ICommandOptions
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string Name { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
}