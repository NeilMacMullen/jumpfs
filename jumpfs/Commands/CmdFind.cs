using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdFind
    {
        public static readonly CommandDescriptor Descriptor = new CommandDescriptor(Run, "find")
            .WithArguments(
                ArgumentDescriptor.Create<string>(Names.Name)
                    .Mandatory()
                    .WithHelpText("name of the bookmark"),
                ArgumentDescriptor.Create<string>(Names.Format)
                    .WithHelpText(@"custom output format:
  %p - full path
  %l - line number 
  %c - column number

specifiers can be combined and separated .  For example:

--format %p:%l:%c

")
            )
            .WithHelpText("locates a bookmark with the specified name and outputs the associated path");

        public static void Run(ParseResults results, ApplicationContext context)
        {
            var name = results.ValueOf<string>(Names.Name);
            var format = results.ValueOf<string>(Names.Format);
            var mark = context.Repo.Find(name);
            var path = context.ToNative(mark.Path);
            if (format.Length == 0)
                context.WriteLine(path);
            else
            {
                format = format
                        .Replace("%p", mark.Path)
                        .Replace("%l", mark.Line.ToString())
                        .Replace("%c", mark.Column.ToString())
                    ;
                context.WriteLine(format);
            }
        }
    }
}