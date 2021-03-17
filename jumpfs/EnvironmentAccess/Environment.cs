using System.IO;

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
            Directory.CreateDirectory(location);
            File.WriteAllText(location, text);
        }

        public string GetEnvironmentVariable(string name) =>
            System.Environment.GetEnvironmentVariable(name) ?? string.Empty;

        public string GetFolderPath(System.Environment.SpecialFolder folderName) =>
            System.Environment.GetFolderPath(folderName);

        public string Cwd() => Directory.GetCurrentDirectory();
    }
}
