using System;
using System.IO;
using Core.Bookmarking;
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
                ArgumentDescriptor.CreateSwitch(Names.Win)
                    .WithHelpText("use windows path"),
                ArgumentDescriptor.Create<string>(Names.Format)
                    .WithHelpText(@"custom output format:
  %f - (default) containing folder when this is a file
  %p - full path
  %l - line number 
  %c - column number
  %N - newline
  %D - Drive specifier (assuming windows path)

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

            var path = results.ValueOf<bool>(Names.Win)
                ? context.ToWindows(mark.Path)
                : context.ToNative(mark.Path);


            var folder = (mark.Type == BookmarkType.File)
                ? Path.GetDirectoryName(path)
                : path;


            if (mark.Name.Length == 0)
            {
                //if we couldn't find the bookmark there is a chance the user gave us an actual path to use
                if (context.Repo.JumpfsEnvironment.FileExists(name))
                {
                    path = name;
                    folder = Path.GetDirectoryName(path);
                }
                else if (context.Repo.JumpfsEnvironment.DirectoryExists(name))
                {
                    path = folder = name;
                }
                else
                {
                    //we've run out of options - it's probably a missing bookmark so return an empty line
                    context.WriteLine("");
                    return;
                }
            }


            var drive = path.Length > 0 ? path.Substring(0, 1) : string.Empty;
            if (format.Length == 0)
            {
                context.WriteLine(folder);
            }
            else
            {
                format = format
                        .Replace("%f", folder)
                        .Replace("%p", path)
                        .Replace("%l", mark.Line.ToString())
                        .Replace("%c", mark.Column.ToString())
                        .Replace("%N", Environment.NewLine)
                        .Replace("%D", drive)
                    ;
                context.WriteLine(format);
            }
        }
    }
}
