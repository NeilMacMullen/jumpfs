using System;

namespace jumpfs.Extensions
{
    public static class StringExtensions
    {
        //split a string by spaces - useful for testing
        public static string[] Tokenise(this string str) => str.Split(' ',
            StringSplitOptions.TrimEntries |
            StringSplitOptions.RemoveEmptyEntries
        );

        public static string WinSlash(this string str) => str.Replace("/", @"\");
        public static string UnixSlash(this string str) => str.Replace(@"\", "/");
    }
}