using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdRemove
    {
        public static readonly CommandDescriptor Descriptor
            = new CommandDescriptor(Run, "remove")
                .WithArguments(
                    ArgumentDescriptor.Create<string>(Names.Name)
                        .WithHelpText("removes the named bookmark")
                )
                .WithHelpText("removes a bookmark");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            var name = results.ValueOf<string>(Names.Name);
            var victims = context.Repo.Remove(name);
            foreach (var v in victims)
            {
                context.WriteLine($"Removed {v.Name} --> {v.Path}");
            }
        }
    }
}
