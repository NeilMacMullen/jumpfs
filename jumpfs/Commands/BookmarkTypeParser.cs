using System;
using System.Linq;
using Core.Bookmarking;

namespace jumpfs.Commands
{
    public static class BookmarkTypeParser
    {
        public static readonly BookmarkType[] FilesystemTypes =
        {
            BookmarkType.File, BookmarkType.Folder
        };

        public static BookmarkType Parse(string bookmarkType)
        {
            if (Enum.TryParse(typeof(BookmarkType), bookmarkType, true, out var parsedType))
            {
                return (BookmarkType) parsedType;
            }

            return BookmarkType.Unknown;
        }

        public static bool IsFileSystem(BookmarkType markType, string name)
        {
            var wantedType = Parse(name);
            return FilesystemTypes.Contains(markType) && Matches(markType, wantedType);
        }

        private static bool Matches(BookmarkType markType, BookmarkType wantedType) =>
            wantedType == BookmarkType.Unknown || (markType == wantedType);

        public static bool Match(BookmarkType markType, string type, BookmarkType requiredType)
        {
            var wantedType = Parse(type);
            return markType == wantedType && wantedType == requiredType;
        }
    }
}
