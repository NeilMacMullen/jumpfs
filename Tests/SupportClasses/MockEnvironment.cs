using System.Collections.Generic;
using System.IO;
using jumpfs;
using jumpfs.EnvironmentAccess;
using Environment = System.Environment;

namespace Tests.SupportClasses
{
    public class MockFileSystem
    {
        private readonly Dictionary<string, string> _files = new();
        private readonly HashSet<string> _folders = new();
        public bool DirectoryExists(string path) => _folders.Contains(path);
        public bool FileExists(string path) => _files.ContainsKey(path);

        public string ReadAllText(string path)
        {
            if (!FileExists(path))
                throw new IOException("File not found");
            return _files[path];
        }

        public void WriteAllText(string path, string text)
        {
            var folder = Path.GetDirectoryName(path);
            _folders.Add(folder);

            _files[path] = text;
        }
    }

    //Mocks up the environment so we can test with a fake bookmark file
    public class MockEnvironment : IEnvironment
    {
        private readonly Dictionary<string, string> _env = new();
        private readonly MockFileSystem _fileSystem;
        private string _cwd = string.Empty;


        public MockEnvironment(ShellType shellType, MockFileSystem fs)
        {
            ShellType = shellType;
            _fileSystem = fs;
        }

        public ShellType ShellType { get; }
        public bool DirectoryExists(string path) => _fileSystem.DirectoryExists(path);

        public bool FileExists(string path) => _fileSystem.FileExists(path);

        public string ReadAllText(string path) => _fileSystem.ReadAllText(path);

        public void WriteAllText(string location, string text)
        {
            _fileSystem.WriteAllText(location, text);
        }


        public string GetEnvironmentVariable(string name) => _env.TryGetValue(name, out var v) ? v : string.Empty;

        public string GetFolderPath(Environment.SpecialFolder folderName) =>
            Environment.GetFolderPath(folderName);

        public string Cwd() => _cwd;


        public string SetEnvironmentVariable(string name, string val) => _env[name] = val;

        public void SetCwd(string s)
        {
            _cwd = s;
        }
    }
}