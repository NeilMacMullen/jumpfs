using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdList
    {
        public static readonly CommandDescriptor Descriptor
            = new CommandDescriptor(Run, "list")
                .WithArguments(
                    ArgumentDescriptor.Create<string>(Names.Match)
                        .WithHelpText("restrict the output to items where the name or path matches the supplied string")
                        .AllowEmpty()
                )
                .WithHelpText("lists all or a subset of stored bookmarks");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            var name = results.ValueOf<string>(Names.Match);
            var marks = context.Repo.List(name);
            foreach (var mark in marks)
            {
                context.WriteLine($"{mark.Name} --> {context.ToNative(mark.Path)}");
            }
        }
    }
}
