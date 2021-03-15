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
            context.WriteLine("Executable file: ..todo");
            context.WriteLine($"Bookmark file: {context.Repo.BookmarkFile}");
            context.WriteLine("Checking for new version");
        }
    }
}
