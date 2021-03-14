using FluentAssertions;
using jumpfs;
using jumpfs.Commands;
using jumpfs.EnvironmentAccess;
using NUnit.Framework;
using Tests.SupportClasses;

namespace Tests
{
    [TestFixture]
    public class FullPathTests
    {
        [Test]
        public void FromWindows()
        {
            //These tests only run on windows
            if (ShellGuesser.IsUnixy())
                return;
            var env = new MockEnvironment(ShellType.PowerShell, new MockFileSystem());
            env.SetCwd(@"C:\a\b");
            var p = new FullPathCalculator(env);
            p.ToAbsolute(@"d:\x").Should().Be(@"d:\x");
            p.ToAbsolute(@"..\x").Should().Be(@"C:\a\x");
        }

        [Test]
        public void FromWslVirtualDrive()
        {
            //These tests only run on *nix
            if (!ShellGuesser.IsUnixy())
                return;
            var env = new MockEnvironment(ShellType.Wsl, new MockFileSystem());
            env.SetCwd(@"/mnt/d/a");
            var p = new FullPathCalculator(env);
            p.ToAbsolute(@"../x").Should().Be(@"/mnt/d/x");
            p.ToAbsolute(@"/x").Should().Be(@"/x");
            p.ToAbsolute(@"D:\x").Should().Be(@"D:\x");
            p.ToAbsolute(@"\\wsl$Ubuntu\var").Should().Be(@"\\wsl$Ubuntu\var");
        }
    }
}