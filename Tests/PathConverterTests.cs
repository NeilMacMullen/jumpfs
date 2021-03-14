using FluentAssertions;
using jumpfs;
using jumpfs.Bookmarking;
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

        private readonly string Unc = @"\\wsl$Ubuntu\";

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
            var p = new PathConverter(Unc);
            var src = @"/var/etc";
            p.ToShell(ShellType.Wsl, src).Should().Be("/var/etc");
        }

        [Test]
        public void InternalPSPath()
        {
            var p = new PathConverter(Unc);
            var src = @"/var/etc";
            p.ToShell(ShellType.PowerShell, src).Should().Be($@"{Unc}var\etc");
        }

        [Test]
        public void UNCPath()
        {
            var p = new PathConverter(Unc);
            var src = @$"{Unc}var\etc";
            p.ToShell(ShellType.Wsl, src).Should().Be("/var/etc");
            p.ToShell(ShellType.PowerShell, src).Should().Be(src);
        }
    }
}