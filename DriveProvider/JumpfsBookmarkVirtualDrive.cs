using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Runtime.CompilerServices;
using jumpfs.Bookmarking;
using jumpfs.EnvironmentAccess;
using jumpfs.Extensions;

namespace DriveProvider
{
    //https://johnkoerner.com/csharp/creating-a-powershell-navigation-provider-4-tab-completion/

    [CmdletProvider("jumpfs", ProviderCapabilities.ExpandWildcards)]
    public class JumpfsBookmarkVirtualDrive : NavigationCmdletProvider, IContentCmdletProvider
    {
        private BookmarkRepository _repo => new BookmarkRepository(new JumpfsEnvironment());


        protected override string[] ExpandPath(string path)
        {
            Debug(path);
            path ??= "";
            path = TranslatePath(path);
            var bmk = _repo.Load();
            return BookmarkRepository.Match(bmk, path)
                .Select(b => b.Name).ToArray();
        }

        protected override bool HasChildItems(string path)
        {
            Debug(path);
            //bookmarks don't have children
            return false;
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            Debug(path);
            _repo.Remove(TranslatePath(path));
        }


        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            Debug(path);
            var bookmarks = _repo.Load()
                .OrderBy(b => b.Name)
                .Select(b => b.Name)
                .ToArray();
            foreach (var b in bookmarks)
                WriteItemObject(b, path, false);
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            Debug($"{path} {recurse}");
            var bookmarks = _repo.Load()
                .OrderBy(b => b.Name);
            foreach (var b in bookmarks)
            {
                WriteItemObject(b, path, false);
            }
        }

        //just say that all paths are valid
        protected override bool IsValidPath(string path) => true;

        private string TranslatePath(string path) => path.Replace(PSDriveInfo.Root, "");

        protected override bool ItemExists(string path)
        {
            Debug(path);
            if (path == null)
                return true;
            path = TranslatePath(path);
            var bookmarks = _repo.Load();
            return string.IsNullOrEmpty(path) || bookmarks.Any(b => b.Name == path);
        }


        //for testing, just return a string
        protected override void GetItem(string path)
        {
            Debug(path);
            var bookmarks = _repo.Load();
            foreach (var bookmark in bookmarks.Where(b => b.Name == TranslatePath(path)))
            {
                WriteItemObject(bookmark, path, false);
            }
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            Debug("");
            return base.NewDrive(drive);
        }

        private void Debug(string message, [CallerMemberName] string caller = "none")
        {
            WriteDebug($"{caller}:  {message}");
        }

        #region CONTENT

        public void ClearContent(string path)
        {
            Debug(path);
            //throw new NotImplementedException();
        }

        public object ClearContentDynamicParameters(string path)
        {
            Debug(path);
            //throw new NotImplementedException();
            return null;
        }

        public IContentReader GetContentReader(string path)
        {
            Debug(path);
            var bookmarks = _repo.Load();
            if (bookmarks.TryGetSingle(b => b.Name == TranslatePath(path), out var hit))
            {
                return new MyContentReader(hit.Path);
            }

            return new MyContentReader(string.Empty);
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            Debug(path);
            return null;
        }

        public IContentWriter GetContentWriter(string path)
        {
            Debug(path);
            //throw new NotImplementedException();
            return null;
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            Debug(path);
            return null;
        }

        #endregion
    }

    public class MyContentReader : IContentReader
    {
        private readonly string _path;

        public MyContentReader(string path) => _path = path;

        public void Dispose()
        {
        }

        public void Close()
        {
        }

        public IList Read(long readCount)
        {
            return new[] {_path}.ToList();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
        }
    }
}
