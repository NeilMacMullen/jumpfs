using System.Text.RegularExpressions;
using jumpfs.Extensions;

namespace jumpfs.Bookmarking
{
    /// <summary>
    ///     Converts paths between Windows and Linux/WSL
    /// </summary>
    /// <remarks>
    ///     Requires some extra logic to cope with WSL
    /// </remarks>
    public class PathConverter
    {
        private readonly string _unc;

        public PathConverter(string unc) => _unc = unc;

        public string ToWsl(string path)
        {
            if (_unc.Length > 0 && path.StartsWith(_unc))
                return path.Substring(_unc.Length - 1).UnixSlash();

            var m = Regex.Match(path, @"^(\w):(.*)");

            if (m.Success)
            {
                var root = m.Groups[1].Value;
                path = $"/mnt/{root.ToLowerInvariant()}" + m.Groups[2].Value;
            }

            return path.UnixSlash();
        }

        public string ToUnc(string path)
        {
            if (path.StartsWith("/"))
            {
                var m = Regex.Match(path, @"^/mnt/(\w)/(.*)");
                if (m.Success)
                {
                    var drv = m.Groups[1].Value;
                    var subPath = m.Groups[2].Value.WinSlash();
                    return $@"{drv}:\{subPath}";
                }

                return $"{_unc}{path.Substring(1).WinSlash()}";
            }

            return path;
        }

        public string ToShell(ShellType environment, string path)
        {
            switch (environment)
            {
                case ShellType.Wsl:
                    return ToWsl(path);
                default: return ToUnc(path);
            }
        }
    }
}