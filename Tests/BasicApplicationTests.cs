using System;
using System.IO;
using System.Text;
using Core;
using Core.Bookmarking;
using Core.EnvironmentAccess;
using Core.Extensions;
using FluentAssertions;
using jumpfs.Commands;
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
            _sb = new StringBuilder();
            _stdout = new StringWriter(_sb);
            _stderr = new StringWriter();
            _env = new MockJumpfsEnvironment(ShellType.PowerShell, new MockFileSystem());
            if (ShellGuesser.IsUnixy())
            {
                _env.SetCwd(@"/usr");
            }
            else
            {
                _env.SetCwd(@"D:\");
            }

            _repo = new BookmarkRepository(_env);
            _context = new ApplicationContext(_repo, _stdout, _stderr, Array.Empty<string>());
        }

        private ApplicationContext _context;
        private BookmarkRepository _repo;
        private StringWriter _stderr;

        private StringWriter _stdout;

        private MockJumpfsEnvironment _env;

        private void Execute(string cmd)
        {
            JumpFs.ExecuteWithContext(cmd.Tokenise(), _context);
        }


        private StringBuilder _sb;
        private string GetStdOut() => _sb.ToString();

        private void CheckOutput(string expected)
        {
            GetStdOut().Trim().Should().EndWith(expected);
            _sb.Clear();
        }

        [Test]
        public void SimpleBookmarking()
        {
            Execute("mark --name here --path apath --literal");
            Execute("find --name here");
            CheckOutput(@"apath");
        }

        [Test]
        public void FindOfMissingBookmarkReturnsEmptyLine()
        {
            Execute("find --name here");
            GetStdOut().Should().Be(EmptyLine);
        }


        [Test]
        public void FindOfMissingBookmarkReturnsEmptyLineEvenWhenFormatSpecified()
        {
            Execute("find --name here --format \"asdfasdf\"");
            GetStdOut().Should().Be(EmptyLine);
        }

        private static readonly string EmptyLine = "" + Environment.NewLine;

        [Test]
        public void RemoveWorks()
        {
            Execute("mark --name here --path apath --literal");
            Execute("remove --name here");
            CheckOutput(@"apath");
            Execute("remove --name here");
            GetStdOut().Should().Be(EmptyLine);
        }


        [Test]
        public void FormattedBookmarking()
        {
            Execute("mark --name here --path apath --line 15 --column 10 --literal");
            Execute("find --name here --format %p:%l:%c");
            CheckOutput(@"apath:15:10");
        }


        [Test]
        public void UrlNotReturnedUnlessTypeSpecified()
        {
            Execute("mark --name here --path http://atest");
            Execute("find --name here");
            GetStdOut().Trim().Should().BeEmpty();
        }

        [Test]
        public void UrlReturnedWhenTypeSpecified()
        {
            Execute("mark --name here --path http://atest");
            Execute("find --name here --type Url");
            CheckOutput(@"http://atest");
        }


        [Test]
        public void ScriptNotReturnedUnlessTypeSpecified()
        {
            Execute("mark --name here --path 'a_script' --type PsCmd");
            Execute("find --name here");
            GetStdOut().Trim().Should().BeEmpty();
        }

        [Test]
        public void ScriptReturnedWhenTypeSpecified()
        {
            Execute("mark --name here --path a_script --type PsCmd");
            Execute("find --name here --type PsCmd");
            CheckOutput(@"a_script");
        }

        [Test]
        public void ScriptNotReturnedInWrongShell()
        {
            Execute("mark --name here --path 'a_script' --type BashCmd");
            Execute("find --name here --type BashCmd");
            GetStdOut().Trim().Should().BeEmpty();
        }
    }
}
