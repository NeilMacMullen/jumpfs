namespace jumpfs.Bookmarking
{
    /// <summary>
    ///     A bookmark entry
    /// </summary>
    public class Bookmark
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int Line { get; set; }
        public int Column { get; set; }
        public BookmarkType Type { get; set; }
    }

    public enum BookmarkType
    {
        Unknown,
        File,
        Folder,

        //Not yet supported
        Url,
        Script
    }
}
