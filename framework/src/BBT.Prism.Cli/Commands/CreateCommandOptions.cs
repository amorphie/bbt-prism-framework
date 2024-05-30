using System.CommandLine;

namespace Commands;

public class CreateCommandOptions : ICommandOptions
{
    // Automatic binding with System.CommandLine.NamingConventionBinder
    public string Type { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
}