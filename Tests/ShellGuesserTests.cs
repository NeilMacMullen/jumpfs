using Core;
using Core.Bookmarking;
using Core.EnvironmentAccess;
using FluentAssertions;
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
            var initialEnv = new MockJumpfsEnvironment(ShellType.PowerShell, new MockFileSystem());
            initialEnv.SetEnvironmentVariable(EnvVariables.ShellOveride, "cmd");
            ShellGuesser
                .GuessShell(initialEnv)
                .Should()
                .Be(ShellType.Cmd);
        }

        [Test]
        public void ShouldGuessIfVarNotPresent()
        {
            var initialEnv = new MockJumpfsEnvironment(ShellType.PowerShell, new MockFileSystem());
            ShellGuesser
                .GuessShell(initialEnv)
                .Should()
                .NotBe(ShellType.Cmd);
        }
    }

    [TestFixture]
    public class RegexTests
    {
        [Test]
        public void Translation()
        {
            RegexTranslator
                .RegexFromPowershell("abc")
                .Should().Be("^abc$");

            RegexTranslator
                .RegexFromPowershell("")
                .Should().Be("^.*$");

            RegexTranslator
                .RegexFromPowershell("a*b*c")
                .Should().Be("^a.*b.*c$");

            RegexTranslator
                .RegexFromPowershell("?a*")
                .Should().Be("^.a.*$");

            RegexTranslator
                .RegexFromPowershell("[a-b]*")
                .Should().Be("^[a-b]*$");

            RegexTranslator
                .RegexFromPowershell("a[a-b]*c?d")
                .Should().Be("^a[a-b]*c.d$");
        }
    }
}
