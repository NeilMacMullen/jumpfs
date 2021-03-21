using Core;
using Core.Bookmarking;
using FluentAssertions;
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
            _jumpfsEnvironment = new MockJumpfsEnvironment(ShellType.PowerShell,
                new MockFileSystem());
            _repo = new BookmarkRepository(_jumpfsEnvironment);
        }

        private BookmarkRepository _repo;
        private MockJumpfsEnvironment _jumpfsEnvironment;

        [Test]
        public void BookMarkCanBeWritten()
        {
            _repo.Mark(BookmarkType.Folder, "a", "a path");
            var b = _repo.Find("a");
            b.Name.Should().Be("a");
            b.Type.Should().Be(BookmarkType.Folder);
        }

        [Test]
        public void BookMarkCanBeChanged()
        {
            _repo.Mark(BookmarkType.Folder, "home", @"path\1");
            _repo.Mark(BookmarkType.Folder, "home", @"path\2");

            var b = _repo.Find("home");
            b.Name.Should().Be("home");
            b.Path.Should().Be(@"path\2");
        }
    }
}
