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
            var exeFolder = AppContext.BaseDirectory;

            context.WriteLine(
                @$"
Jumpfs v{GitVersionInformation.SemVer}: 
- Executable location: {exeFolder}
- Bookmark file:       {context.Repo.BookmarkFile}

Runtime Info:
 - System:             {RuntimeEnvironment.GetSystemVersion()}
 - Architecture:       {RuntimeInformation.OSArchitecture}
 - OS:                 {RuntimeInformation.OSDescription}
 - Shell:              {context.Repo.Environment.ShellType}

Useful links:
 - jumpfs homepage:    https://github.com/NeilMacMullen/jumpfs
 - Chat and questions: https://gitter.im/jumpfs/community
            ");


            context.WriteLine("Checking for new version...");
            var t = UpgradeManager.GetLatestVersion();
            t.Wait();
            var latestVersion = t.Result;
            context.WriteLine(
                latestVersion.Supersedes(GitVersionInformation.SemVer)
                    ? @$"
  An Upgrade to version {latestVersion.Version} is available.
  Please visit {UpgradeManager.ReleaseSite} for download.
"
                    : @"
  You are running the latest version."
            );
        }
    }
}
