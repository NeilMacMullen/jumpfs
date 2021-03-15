using System.IO;
using jumpfs.EnvironmentAccess;

namespace jumpfs.Commands
{
    /// <summary>
    ///     Allows conversion between absolute and relative paths
    /// </summary>
    public class FullPathCalculator
    {
        public FullPathCalculator(IEnvironment env) => Env = env;
        public IEnvironment Env { get; set; }

        /// <summary>
        ///     Returns an absolute (rooted) path
        /// </summary>
        /// <remarks>
        ///     If a relative path is passed in we assume it is relative to the root
        /// </remarks>
        public string ToAbsolute(string path)
        {
            var root = Env.Cwd();
            if (path.Length == 0) return root;
            //Path.GetFullPath does different things under unix and windows
            //so we need to do some run-time adjustment!
            if (ShellGuesser.IsUnixy())
            {
                //under wls we have want to be able to cope with the relative path
                //being either....
                // a relative path ../x
                // an absolute path /var
                // a wsl path such as c:\users\aaa
                // a wsl path to internal drive such \\wsl$\Ubuntu\var


                if (path.StartsWith("/"))
                    return path;


                if (path.StartsWith(@"\\"))
                    return path;
                if (path.Length > 1 && path[1] == ':')
                    return path;
                return Path.GetFullPath(path, root);
            }

            //there's less to worry about on windows!
            return Path.GetFullPath(path, root);
        }
    }
}
