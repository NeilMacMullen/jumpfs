using System;
using jumpfs.Bookmarking;
using jumpfs.CommandLineParsing;
using jumpfs.EnvironmentAccess;

namespace jumpfs.Commands
{
    public class JumpFs
    {
        public static CommandLineParser CreateParser()
        {
            var parser = new CommandLineParser(
                CmdShowArgs.Descriptor,
                CmdInfo.Descriptor,
                CmdMark.Descriptor,
                CmdFind.Descriptor,
                CmdList.Descriptor,
                CmdRemove.Descriptor
            );
            return parser;
        }

        public static ApplicationContext CliContext()
        {
            var outputStream = Console.Out;
            var errorStream = Console.Error;

            var repo = new BookmarkRepository(new JumpfsEnvironment());
            return new ApplicationContext(repo, outputStream, errorStream);
        }


        public static void ExecuteWithContext(string[] args, ApplicationContext context)
        {
            var parser = CreateParser();
            var results = parser.Parse(args);
            if (results.IsSuccess)
                results.Execute(context);
            else
            {
                if (results.CommandDescriptor.Name.Length != 0)
                    context.ErrorStream.WriteLine($"Command: {results.CommandDescriptor.Name}");
                context.ErrorStream.WriteLine($"Error: {results.Message}");
            }
        }
    }
}
