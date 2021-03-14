using FluentAssertions;
using jumpfs;
using jumpfs.Bookmarking;
using NUnit.Framework;
using Tests.SupportClasses;

namespace Tests
{
    [TestFixture]
    public class BookMarkTests
    {
        [SetUp]
        public void Setup()
        {
            _environment = new MockEnvironment(ShellType.PowerShell,
                new MockFileSystem());
            _repo = new BookmarkRepository(_environment);
        }

        private BookmarkRepository _repo;
        private MockEnvironment _environment;

        [Test]
        public void BookMarkCanBeWritten()
        {
            _repo.Mark("a", "a path");
            var b = _repo.Find("a");
            b.Name.Should().Be("a");
        }

        [Test]
        public void BookMarkCanBeChanged()
        {
            _repo.Mark("home", @"path\1");
            _repo.Mark("home", @"path\2");

            var b = _repo.Find("home");
            b.Name.Should().Be("home");
            b.Path.Should().Be(@"path\2");
        }
    }
}