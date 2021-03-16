using System;
using System.Runtime.InteropServices;
using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdInfo
    {
        public static readonly CommandDescriptor Descriptor =
            new CommandDescriptor(Run, "info")
                .WithHelpText("shows information about the application and environment it is running in");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            context.WriteLine($"System: {RuntimeEnvironment.GetSystemVersion()}");
            context.WriteLine($"Architecture: {RuntimeInformation.OSArchitecture}");
            context.WriteLine($"OS: {RuntimeInformation.OSDescription}");
            context.WriteLine($"Shell: {context.Repo.Environment.ShellType}");
            var exeFolder = AppContext.BaseDirectory;
            context.WriteLine($"Executable location: {exeFolder}");
            context.WriteLine($"Bookmark file: {context.Repo.BookmarkFile}");


            context.WriteLine(
                @$"
Version {GitVersionInformation.SemVer} built {GitVersionInformation.CommitDate}   commit {GitVersionInformation.ShortSha} 

Useful links:
 - jumpfs homepage:        https://github.com/NeilMacMullen/jumpfs
 - Chat and questions:     https://gitter.im/jumpfs/community
");
            context.WriteLine("Checking for new version...");
            var t = UpgradeManager.GetLatestVersion();
            t.Wait();
            var latestVersion = t.Result;
            if (latestVersion.Supersedes(GitVersionInformation.SemVer))
            {
                context.WriteLine($"Upgrade to version {latestVersion.Version} available");
                context.WriteLine($"Please visit {UpgradeManager.ReleaseSite} for download");
            }
            else context.WriteLine("You are running the latest version");
        }
    }
}
