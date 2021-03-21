using System;

namespace jumpfs.Bookmarking
{
    public class BookmarkSet
    {
        public JumpFsConfiguration Configuration { get; set; } = JumpFsConfiguration.Empty;
        public Bookmark[] Bookmarks { get; set; } = Array.Empty<Bookmark>();
    }
}
