using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using jumpfs.EnvironmentAccess;

namespace jumpfs.Bookmarking
{
    /// <summary>
    ///     A repository for bookmarks
    /// </summary>
    /// <remarks>
    ///     The repository makes certain assumptions about the persistence mechanism
    /// </remarks>
    public class BookmarkRepository
    {
        public readonly string BookmarkFile;
        public readonly IEnvironment Environment;

        public BookmarkRepository(IEnvironment environment)
        {
            Environment = environment;
            BookmarkFile = Path.Combine(Folder, "jumpfs", "bookmarks.json");
        }

        public string Folder =>
            (Environment.ShellType == ShellType.Wsl)
                ? Environment.GetEnvironmentVariable(EnvVariables.WslEnvVar).Trim()
                : Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

        public Bookmark[] Load()
        {
            var text = !Environment.FileExists(BookmarkFile) ? "[]" : Environment.ReadAllText(BookmarkFile);
            return JsonSerializer.Deserialize<Bookmark[]>(text);
        }

        public Bookmark[] List(string match)
        {
            var all = Load();
            var matches = all.Where(m => m.Name.Contains(match) || m.Path.Contains(match)).ToArray();
            return matches;
        }


        public void Save(Bookmark[] bookmarks)
        {
            var text = JsonSerializer.Serialize(bookmarks, new JsonSerializerOptions {WriteIndented = true});
            Environment.WriteAllText(BookmarkFile, text);
        }

        public void Mark(string name, string path) => Mark(name, path, 0, 0);

        public void Mark(string name, string path, int line, int column)
        {
            var marks = Load();
            var existing = marks.SingleOrDefault(m => m.Name == name);
            if (existing == null)
            {
                existing = new Bookmark {Path = path};
                marks = marks.Append(existing).ToArray();
            }

            existing.Path = path;
            existing.Name = name;
            existing.Line = line;
            existing.Column = column;
            Save(marks);
        }

        public Bookmark Find(string name)
        {
            var marks = Load();
            var existing = marks.SingleOrDefault(m => m.Name == name)
                           ?? new Bookmark();
            return existing;
        }
    }

    public class BookmarkSet
    {
        public JumpFsConfiguration Configuration { get; set; } = JumpFsConfiguration.Empty;
        public Bookmark[] Bookmarks { get; set; } = Array.Empty<Bookmark>();
    }

    public class JumpFsConfiguration
    {
        public static readonly JumpFsConfiguration Empty = new();
    }
}