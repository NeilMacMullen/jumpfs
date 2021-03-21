using System;
using System.Collections.Generic;
using System.IO;
using Core;
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

"), ArgumentDescriptor.Create<string>(Names.Type)
                    .AllowEmpty()
                    .WithHelpText("type (default restricts output to filesystem)")
            )
            .WithHelpText("locates a bookmark with the specified name and outputs the associated path");

        public static void Run(ParseResults results, ApplicationContext context)
        {
            var type = results.ValueOf<string>(Names.Type);


            var name = results.ValueOf<string>(Names.Name);
            var mark = context.Repo.Find(name);

            var format = results.ValueOf<string>(Names.Format);

            if (BookmarkTypeParser.IsFileSystem(mark.Type, type))
            {
                if (EmitFilesystem(results, context, mark, name, format))
                    return;
            }

            if (BookmarkTypeParser.Match(mark.Type, type, BookmarkType.Url))
            {
                //no translation needed for URLs
                context.WriteLine(mark.Path);
                return;
            }

            //ensure we only emit scripts for the correct shell
            var scriptTypes = new Dictionary<ShellType, BookmarkType>
            {
                [ShellType.PowerShell] = BookmarkType.PsCmd,
                [ShellType.Linux] = BookmarkType.BashCmd,
                [ShellType.Wsl] = BookmarkType.BashCmd,
                [ShellType.Cmd] = BookmarkType.DosCmd,
            };

            if (scriptTypes.TryGetValue(context.Repo.JumpfsEnvironment.ShellType, out var scriptType))
            {
                if (BookmarkTypeParser.Match(mark.Type, type, scriptType))
                {
                    //no translation needed for Script
                    context.WriteLine(mark.Path);
                    return;
                }
            }

            context.WriteLine("");
        }

        private static bool EmitFilesystem(ParseResults results, ApplicationContext context, Bookmark mark, string name,
            string format)
        {
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
                    return false;
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

            return true;
        }
    }
}
