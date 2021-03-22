using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdCheckVersion
    {
        public static readonly CommandDescriptor Descriptor =
            new CommandDescriptor(Run, "checkVersion")
                .WithArguments(
                    ArgumentDescriptor.CreateSwitch("quiet")
                )
                .WithHelpText("checks for a new version");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            var quiet = results.ValueOf<bool>(Names.Quiet);
            UpgradeManager.CheckAndWarnOfNewVersion(context.OutputStream, quiet);
        }
    }
}
