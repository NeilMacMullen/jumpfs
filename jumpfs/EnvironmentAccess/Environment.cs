using System;
using System.IO;
using System.Runtime.InteropServices;
using jumpfs.Bookmarking;

namespace jumpfs.EnvironmentAccess
{
    public class Environment : IEnvironment
    {
        public Environment() => ShellType = ShellGuesser.GuessShell(this);

        public ShellType ShellType { get; init; }
        public bool DirectoryExists(string path) => Directory.Exists(path);

        public bool FileExists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);

        public void WriteAllText(string location, string text)
        {
            File.WriteAllText(location, text);
        }

        public string GetEnvironmentVariable(string name) =>
            System.Environment.GetEnvironmentVariable(name) ?? string.Empty;

        public string GetFolderPath(System.Environment.SpecialFolder folderName) =>
            System.Environment.GetFolderPath(folderName);

        public string Cwd() => Directory.GetCurrentDirectory();

        private ShellType GuessShell()
        {
            //if the user has not specified the shell, try to guess it from environmental information
            var forcedEnv = GetEnvironmentVariable(EnvVariables.ShellOveride);
            return Enum.TryParse(typeof(ShellType), forcedEnv, true, out var shell)
                ? (ShellType) shell
                : RuntimeInformation.OSDescription.Contains("Linux")
                    ? ShellType.Wsl
                    : ShellType.PowerShell;
        }
    }
}