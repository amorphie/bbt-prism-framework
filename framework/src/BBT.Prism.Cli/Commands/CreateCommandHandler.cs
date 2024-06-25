using System;
using System.CommandLine;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Helpers;
using ShellProgressBar;

namespace Commands;

public class CreateCommandHandler(IConsole console) : ICommandHandler<CreateCommandOptions, CreateCommandArguments>
{
    // Inject anything here, no more hard dependency on System.CommandLine
    private const string RepoUrl = "https://github.com/amorphie/bbt-prism-framework.git";

    public async Task<int> HandleAsync(CreateCommandOptions options, CreateCommandArguments? arguments,
        CancellationToken cancellationToken)
    {
        if (arguments != null)
        {
            console.WriteLine($"Project name: {arguments.Name}");
        }

        console.WriteLine($"Type: {options.Type}");
        console.WriteLine($"Output: {options.Output}");

        var tempDir = CreateTempDirectory();

        var gitCloneResult = await RunGitCloneAsync(RepoUrl, tempDir);

        if (gitCloneResult != 0)
        {
            console.WriteLine("Failed to download the project.");
            return gitCloneResult;
        }

        try
        {
            RunSolutionRenamer(tempDir, arguments!.Name, options.Output);
            console.WriteLine($"{arguments!.Name} project created.");
        }
        catch (Exception e)
        {
            console.WriteLine(e.Message);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }

        return 1;
    }

    private async Task<int> RunGitCloneAsync(string repoUrl, string tempDir)
    {
        var processInfo = new ProcessStartInfo("git", $"clone {repoUrl} {tempDir}") {
            RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true
        };

        var process = Process.Start(processInfo);

        if (process == null)
        {
            console.WriteLine(
                "git cli is not installed or added to the environment variable. For installation <https://git-scm.com/book/en/v2/Getting-Started-The-Command-Line>");
            return 1;
        }
        
        var totalLines = 100;
        var options = new ProgressBarOptions
        {
            ForegroundColor = ConsoleColor.Yellow,
            BackgroundColor = ConsoleColor.DarkGray,
            ProgressCharacter = '─'
        };

        using (var pbar = new ProgressBar(totalLines, "Downloading project...", options))
        {
            int linesProcessed = 0;
            while (!process.HasExited)
            {
                var line = await process.StandardOutput.ReadLineAsync();
                if (line != null)
                {
                    linesProcessed++;
                    pbar.Tick();
                }
            }
            
            var error = await process.StandardError.ReadToEndAsync();
            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
            }
        }

        return process.ExitCode;
    }

    private void RunSolutionRenamer(string tempDir, string projectName, string outputPath)
    {
        var templatePath = Path.Combine(tempDir, "templates", "api");
        var solutionRenamer = new SolutionRenamer(
            templatePath, "BBT", "MyProjectName", "BBT", projectName);

        solutionRenamer.Run();
        CopyProject(templatePath, outputPath);
    }

    private void CopyProject(string sourceDir, string destinationDir)
    {
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
        }

        if (!Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
        }

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var destFile = Path.Combine(destinationDir, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var subdir in Directory.GetDirectories(sourceDir))
        {
            var destSubdir = Path.Combine(destinationDir, Path.GetFileName(subdir));
            CopyProject(subdir, destSubdir);
        }
    }

    private string CreateTempDirectory()
    {
        var tempDir = Path.Combine(
            Path.GetTempPath(),
            Path.GetFileNameWithoutExtension(
                "bbt-cli"
            )
        );

        if (!Directory.Exists(tempDir))
        {
            Directory.CreateDirectory(tempDir);
        }
        else
        {
            Directory.Delete(tempDir, true);
            Directory.CreateDirectory(tempDir);
        }

        return tempDir;
    }
}