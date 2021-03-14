using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdShowArgs
    {
        public static readonly CommandDescriptor Descriptor
            = new CommandDescriptor(Run, "args")
                .WithHelpText(
                    @"writes out the arguments supplied to the program
This can be useful when debugging interpolation issues.");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            context.WriteLine("Received arguments...");
            foreach (var a in context.Args)
                context.WriteLine($"  '{a}'");
        }
    }
}