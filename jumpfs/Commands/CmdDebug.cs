using System.IO;
using jumpfs.Bookmarking;
using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdDebug
    {
        public static readonly CommandDescriptor Descriptor = new CommandDescriptor(Run, "debug")
            .WithArguments(
                ArgumentDescriptor.Create<string>(Names.Path)
                    .WithHelpText("file or folder name")
            )
            .WithHelpText("shows information about the supplied path");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            var path = results.ValueOf<string>(Names.Path);

            var env = context.Repo.Environment;
            context.WriteLine($"cwd '{env.Cwd()}");
            var pm = new FullPathCalculator(env);
            var fullPath = pm.ToAbsolute(path);
            context.WriteLine($"path '{path}'  ->  '{fullPath}'");

            var isFile = File.Exists(fullPath);
            var isDirectory = Directory.Exists(fullPath);
            context.WriteLine($"isFile:{isFile} isDirectory:{isDirectory}");
            if (env.ShellType == ShellType.Wsl && !fullPath.StartsWith("/mnt/"))
            {
                var root = env.GetEnvironmentVariable(EnvVariables.WslRootVar);
                context.WriteLine($"WSL root {root}");
                //can't use path combine
                var cp = root + fullPath;
                context.WriteLine($"New path {cp}");
            }
        }
    }
}