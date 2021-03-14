using System.IO;
using FluentAssertions;
using jumpfs;
using jumpfs.Bookmarking;
using jumpfs.Commands;
using jumpfs.EnvironmentAccess;
using jumpfs.Extensions;
using NUnit.Framework;
using Tests.SupportClasses;

namespace Tests
{
    [TestFixture]
    public class CrossShellTests
    {
        [SetUp]
        public void Setup()
        {
            _stdout = new StringWriter();
            _stderr = new StringWriter();
            _fs = new MockFileSystem();
            var winMock = new MockEnvironment(ShellType.PowerShell, _fs);
            winMock.SetCwd(@"C:\");
            var winRepo = new BookmarkRepository(winMock);
            _win = new ApplicationContext(winRepo, _stdout, _stderr);
            var wslMock = new MockEnvironment(ShellType.Wsl, _fs);
            wslMock.SetCwd("/usr");
            wslMock.SetEnvironmentVariable(
                EnvVariables.WslEnvVar,
                winRepo.Folder
            );
            wslMock.SetEnvironmentVariable(
                EnvVariables.WslRootVar,
                @"\\wsl$\Ubuntu\"
            );
            var wslRepo = new BookmarkRepository(wslMock);

            _wsl = new ApplicationContext(wslRepo, _stdout, _stderr);
        }

        private StringWriter _stdout;
        private StringWriter _stderr;

        private ApplicationContext _win;
        private MockFileSystem _fs;
        private ApplicationContext _wsl;


        private void Execute(ApplicationContext context, string cmd)
        {
            JumpFs.ExecuteWithContext(cmd.Tokenise(), context);
        }

        private string GetStdOut() => _stdout.ToString();
        private void CheckOutput(string expected) => GetStdOut().Trim().Should().Be(expected);

        [Test]
        public void SimpleBookmarking()
        {
            Execute(_win, @"mark -name here -path C:/thepath -literal");
            Execute(_wsl, "find -name here");
            CheckOutput("/mnt/c/thepath");
        }

        [Test]
        public void Win2WslNameTranslation()
        {
            Execute(_win, "mark -name here -path D:/a/b -literal");
            Execute(_wsl, "find -name here");
            CheckOutput("/mnt/d/a/b");
        }

        [Test]
        public void WslToWinNameTranslation()
        {
            Execute(_wsl, "mark -name here -path /mnt/d/a/b -literal");
            Execute(_win, "find -name here");
            CheckOutput(@"d:\a\b");
        }

        [Test]
        public void WslToWinNameTranslation2()
        {
            Execute(_wsl, @"mark -name here -path \\wsl$\Ubuntu\etc\x -literal");
            Execute(_win, "find -name here");
            CheckOutput(@"\\wsl$\Ubuntu\etc\x");
        }

        [Test]
        public void TrueUnixWslToWinNameTranslation()
        {
            if (ShellGuesser.IsUnixy())
            {
                Execute(_wsl, "mark -name here -path /etc/x");
                Execute(_win, "find -name here");
                CheckOutput(@"\\wsl$\Ubuntu\etc\x");
            }
        }
    }
}