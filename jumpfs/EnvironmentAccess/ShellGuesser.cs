using System;
using System.Runtime.InteropServices;
using jumpfs.Bookmarking;

namespace jumpfs.EnvironmentAccess
{
    public static class ShellGuesser
    {
        public static ShellType GuessShell(IEnvironment env)
        {
            //if the user has not specified the shell, try to guess it from environmental information
            var forcedEnv = env.GetEnvironmentVariable(EnvVariables.ShellOveride);
            return Enum.TryParse(typeof(ShellType), forcedEnv, true, out var shell)
                ? (ShellType) shell
                : RuntimeInformation.OSDescription.Contains("Linux")
                    ? ShellType.Wsl
                    : ShellType.PowerShell;
        }

        public static bool IsUnixy() =>
        (
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
            RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)
        );
    }
}