using FluentAssertions;
using jumpfs;
using jumpfs.Bookmarking;
using jumpfs.EnvironmentAccess;
using NUnit.Framework;
using Tests.SupportClasses;

namespace Tests
{
    [TestFixture]
    public class ShellGuesserTests
    {
        [Test]
        public void ShouldUseVarIfPresent()
        {
            var initialEnv = new MockEnvironment(ShellType.PowerShell, new MockFileSystem());
            initialEnv.SetEnvironmentVariable(EnvVariables.ShellOveride, "cmd");
            ShellGuesser
                .GuessShell(initialEnv)
                .Should()
                .Be(ShellType.Cmd);
        }

        [Test]
        public void ShouldGuessIfVarNotPresent()
        {
            var initialEnv = new MockEnvironment(ShellType.PowerShell, new MockFileSystem());
            ShellGuesser
                .GuessShell(initialEnv)
                .Should()
                .NotBe(ShellType.Cmd);
        }
    }
}