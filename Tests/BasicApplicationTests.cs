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
    public class BasicApplicationTests
    {
        [SetUp]
        public void Setup()
        {
            _stdout = new StringWriter();
            _stderr = new StringWriter();
            _env = new MockEnvironment(ShellType.PowerShell, new MockFileSystem());
            if (ShellGuesser.IsUnixy())
            {
                _env.SetCwd(@"/usr");
            }
            else
            {
                _env.SetCwd(@"D:\");
            }

            _repo = new BookmarkRepository(_env);
            _context = new ApplicationContext(_repo, _stdout, _stderr);
        }

        private ApplicationContext _context;
        private BookmarkRepository _repo;
        private StringWriter _stderr;

        private StringWriter _stdout;
        private MockEnvironment _env;

        private void Execute(string cmd)
        {
            JumpFs.ExecuteWithContext(cmd.Tokenise(), _context);
        }

        private string GetStdOut() => _stdout.ToString();
        private void CheckOutput(string expected) => GetStdOut().Trim().Should().EndWith(expected);

        [Test]
        public void SimpleBookmarking()
        {
            Execute("mark --name here --path apath --literal");
            Execute("find --name here");
            CheckOutput(@"apath");
        }

        [Test]
        public void FormattedBookmarking()
        {
            Execute("mark --name here --path apath --line 15 --column 10 --literal");
            Execute("find --name here --format %p:%l:%c");
            CheckOutput(@"apath:15:10");
        }
    }
}
