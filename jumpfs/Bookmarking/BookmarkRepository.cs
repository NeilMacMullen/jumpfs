using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using jumpfs.EnvironmentAccess;
using jumpfs.Extensions;

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

        public void Mark(BookmarkType type, string name, string path) => Mark(type, name, path, 0, 0);

        public void Mark(BookmarkType type, string name, string path, int line, int column)
        {
            var marks = Load();
            var existing = marks.SingleOrDefault(m => m.Name == name);
            if (existing == null)
            {
                existing = new Bookmark {Path = path};
                marks = marks.Append(existing).ToArray();
            }

            existing.Type = type;
            existing.Path = path;
            existing.Name = name;
            existing.Line = line;
            existing.Column = column;
            Save(marks);
        }

        public Bookmark Find(string name)
        {
            var marks = Load();
            return marks.SingleOr(m => m.Name == name, new Bookmark());
        }

        public Bookmark[] Remove(string name)
        {
            var marks = Load();
            var victim = marks.SingleOr(m => m.Name == name, new Bookmark());
            marks = marks.Where(m => m != victim).ToArray();
            Save(marks);
            return new[] {victim}.Where(v => v.Name.Length > 0).ToArray();
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
