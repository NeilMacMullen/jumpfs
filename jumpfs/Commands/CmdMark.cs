using jumpfs.Bookmarking;
using jumpfs.CommandLineParsing;

namespace jumpfs.Commands
{
    public class CmdMark
    {
        public static readonly CommandDescriptor Descriptor = new CommandDescriptor(Run, "mark")
            .WithArguments(
                ArgumentDescriptor.Create<string>(Names.Name)
                    .WithHelpText("name of the bookmark"),
                ArgumentDescriptor.Create<string>(Names.Path)
                    .AllowEmpty()
                    .WithHelpText("file or folder name"),
                ArgumentDescriptor.Create<int>(Names.Line)
                    .AllowEmpty()
                    .WithHelpText("line number"),
                ArgumentDescriptor.Create<int>(Names.Column)
                    .AllowEmpty()
                    .WithHelpText("column number"),
                ArgumentDescriptor.CreateSwitch(Names.Literal)
                    .WithHelpText("use the path as provided rather than trying to turn it into an absolute path")
            )
            .WithHelpText("adds a bookmark.  By default this is considered to be a folder");

        private static void Run(ParseResults results, ApplicationContext context)
        {
            var name = results.ValueOf<string>(Names.Name);
            var path = results.ValueOf<string>(Names.Path);
            var line = results.ValueOf<int>(Names.Line);
            var column = results.ValueOf<int>(Names.Column);

            if (!results.ValueOf<bool>(Names.Literal))
                path = context.ToAbsolutePath(path);
            var type = context.Repo.Environment.FileExists(path) ? BookmarkType.File : BookmarkType.Folder;
            context.Repo.Mark(type, name, path, line, column);
        }
    }
}
