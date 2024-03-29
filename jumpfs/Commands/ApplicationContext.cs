﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.Bookmarking;

namespace jumpfs.Commands
{
    public class ApplicationContext
    {
        private readonly PathConverter _pathConverter;
        public readonly string[] Args;
        public readonly TextWriter ErrorStream;
        public readonly TextWriter OutputStream;
        public readonly BookmarkRepository Repo;

        public ApplicationContext(
            BookmarkRepository repo,
            TextWriter outputStream,
            TextWriter errorStream,
            IEnumerable<string> args)
        {
            Repo = repo;
            Args = args.ToArray();
            OutputStream = outputStream;
            ErrorStream = errorStream;
            _pathConverter = new PathConverter(repo.JumpfsEnvironment.GetEnvironmentVariable(EnvVariables.WslRootVar));
        }

        public void WriteLine(string str) => OutputStream.WriteLine(str);
        public string ToNative(string path) => _pathConverter.ToShell(Repo.JumpfsEnvironment.ShellType, path);
        public string ToWindows(string path) => _pathConverter.ToShell(ShellType.PowerShell, path);

        public string ToAbsolutePath(string path)
        {
            var pm = new FullPathCalculator(Repo.JumpfsEnvironment);
            var abs = pm.ToAbsolute(path);
            return _pathConverter.ToShell(ShellType.PowerShell, abs);
        }
    }
}
