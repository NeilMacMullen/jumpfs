namespace jumpfs
{
    /// <summary>
    ///     These are the environments we currently know how to run under
    /// </summary>
    public enum ShellType
    {
        PowerShell,
        Cmd,
        Wsl,

        //currently Linux is treated the same as WSL
        Linux
    }
}
