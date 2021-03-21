using Core;
using Core.Bookmarking;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PathConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private readonly string _unc = @"\\wsl$Ubuntu\";

        [Test]
        public void RootedPath()
        {
            var p = new PathConverter(string.Empty);
            var src = @"C:\a\b\c";
            p.ToShell(ShellType.Wsl, src).Should().Be("/mnt/c/a/b/c");
            p.ToShell(ShellType.PowerShell, src).Should().Be(src);
        }

        [Test]
        public void InternalWslPath()
        {
            var p = new PathConverter(_unc);
            var src = @"/var/etc";
            p.ToShell(ShellType.Wsl, src).Should().Be("/var/etc");
        }

        [Test]
        public void InternalPsPath()
        {
            var p = new PathConverter(_unc);
            var src = @"/var/etc";
            p.ToShell(ShellType.PowerShell, src).Should().Be($@"{_unc}var\etc");
        }

        [Test]
        public void UncPath()
        {
            var p = new PathConverter(_unc);
            var src = @$"{_unc}var\etc";
            p.ToShell(ShellType.Wsl, src).Should().Be("/var/etc");
            p.ToShell(ShellType.PowerShell, src).Should().Be(src);
        }
    }
}
