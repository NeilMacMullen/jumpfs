using System;
using System.IO;

namespace Core.EnvironmentAccess
{
    public class JumpfsEnvironment : IJumpfsEnvironment
    {
        public JumpfsEnvironment() => ShellType = ShellGuesser.GuessShell(this);

        public ShellType ShellType { get; }
        public bool DirectoryExists(string path) => Directory.Exists(path);

        public bool FileExists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);

        public void WriteAllText(string location, string text)
        {
            var jumpsFolder = Path.GetDirectoryName(location);
            Directory.CreateDirectory(jumpsFolder);
            File.WriteAllText(location, text);
        }

        public string GetEnvironmentVariable(string name) =>
            Environment.GetEnvironmentVariable(name) ?? string.Empty;

        public string GetFolderPath(Environment.SpecialFolder folderName) =>
            Environment.GetFolderPath(folderName);

        public string Cwd() => Directory.GetCurrentDirectory();
    }
}
