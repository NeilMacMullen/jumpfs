using System.Text.RegularExpressions;

namespace jumpfs.Bookmarking
{
    public static class RegexTranslator
    {
        public static string RegexFromPowershell(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                search = "*";
            //we need to be a little careful about how we perform the
            //substitutions
            search = search
                .Replace("?", "!SINGLE!")
                .Replace("]*", "!RANGE!")
                .Replace("*", "!GLOB!")
                .Replace("[", "!LBRACE!");
            search = Regex.Escape(search)
                .Replace("!LBRACE!", "[")
                .Replace("!GLOB!", ".*")
                .Replace("!SINGLE!", ".")
                .Replace("!RANGE!", "]*");
            search = $"^{search}$";
            return search;
        }
    }
}
