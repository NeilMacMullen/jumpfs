﻿using System;
using System.Runtime.InteropServices;
using Core.Bookmarking;

namespace Core.EnvironmentAccess
{
    public static class ShellGuesser
    {
        public static ShellType GuessShell(IJumpfsEnvironment env)
        {
            //if the user has not specified the shell, try to guess it from environmental information
            var forcedEnv = env.GetEnvironmentVariable(EnvVariables.ShellOveride);
            return Enum.TryParse(typeof(ShellType), forcedEnv, true, out var shell)
                // ReSharper disable once PossibleNullReferenceException
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
