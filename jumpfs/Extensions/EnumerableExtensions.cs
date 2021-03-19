using System;
using System.Collections.Generic;
using System.Linq;

namespace jumpfs.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Attempts to get the one and only item in a set
        /// </summary>
        public static bool TryGetSingle<T>(this IEnumerable<T> items, Func<T, bool> selector, out T selected)
        {
            selected = default;
            var matches = items.Where(selector).ToArray();
            if (matches.Length != 1)
                return false;
            selected = matches.First();
            return true;
        }

        public static T SingleOr<T>(this IEnumerable<T> items, Func<T, bool> selector, T fallback)
            => items.TryGetSingle(selector, out var s) ? s : fallback;
    }
}
