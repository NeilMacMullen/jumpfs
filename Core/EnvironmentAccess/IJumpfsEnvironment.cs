﻿using System;

namespace Core.EnvironmentAccess
{
    /// <summary>
    ///     Provides a way of abstracting the file system and environment which is handy for testing
    /// </summary>
    public interface IJumpfsEnvironment
    {
        ShellType ShellType { get; }
        bool DirectoryExists(string path);
        bool FileExists(string path);
        string ReadAllText(string path);
        void WriteAllText(string location, string text);
        public string GetEnvironmentVariable(string name);
        string GetFolderPath(Environment.SpecialFolder folderName);
        public string Cwd();
    }
}
