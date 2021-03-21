using System.Collections;
using System.Collections.ObjectModel;
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

        //we are an item container if at the root
        protected override bool IsItemContainer(string path)
        {
            Debug($"{path}");
            return TranslatePath(path) == "";
        }


        //just say that all paths are valid
        protected override bool IsValidPath(string path) => true;

        private string TranslatePath(string path) => path == null ? "*" : path.Replace(PSDriveInfo.Root, "");

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
                return new BookmarkContentReader(hit.Path);
            }

            return new BookmarkContentReader(string.Empty);
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            Debug(path);
            return null;
        }

        public IContentWriter GetContentWriter(string path)
        {
            Debug(path);
            return null;
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            Debug(path);
            return null;
        }

        #endregion

        #region tracing - no overridden code here

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            Debug("");
            return base.GetChildItemsDynamicParameters(path, recurse);
        }

        protected override void ClearItem(string path)
        {
            Debug("");
            base.ClearItem(path);
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            Debug("");
            return base.ClearItemDynamicParameters(path);
        }

        protected override bool ConvertPath(string path, string filter, ref string updatedPath,
            ref string updatedFilter)
        {
            Debug("");
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }


        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            Debug("");
            base.CopyItem(path, copyPath, recurse);
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            Debug("");
            return base.CopyItemDynamicParameters(path, destination, recurse);
        }

        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            Debug("");
            base.GetChildItems(path, recurse, depth);
        }

        protected override string GetChildName(string path)
        {
            Debug("");
            return base.GetChildName(path);
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            Debug("");
            return base.GetChildNamesDynamicParameters(path);
        }

        protected override object GetItemDynamicParameters(string path)
        {
            Debug("");
            return base.GetItemDynamicParameters(path);
        }

        protected override string GetParentPath(string path, string root)
        {
            Debug("");
            return base.GetParentPath(path, root);
        }

        public override string GetResourceString(string baseName, string resourceId)
        {
            Debug("");
            return base.GetResourceString(baseName, resourceId);
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            Debug("");
            return base.InitializeDefaultDrives();
        }

        protected override void InvokeDefaultAction(string path)
        {
            Debug("");
            base.InvokeDefaultAction(path);
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            Debug("");
            return base.InvokeDefaultActionDynamicParameters(path);
        }


        protected override object ItemExistsDynamicParameters(string path)
        {
            Debug("");
            return base.ItemExistsDynamicParameters(path);
        }


        protected override string MakePath(string parent, string child)
        {
            Debug($"parent:'{parent}' child:'{child}'");
            return base.MakePath(parent, child);
        }

        protected override void MoveItem(string path, string destination)
        {
            Debug("");
            base.MoveItem(path, destination);
        }

        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            Debug("");
            return base.MoveItemDynamicParameters(path, destination);
        }

        protected override object NewDriveDynamicParameters()
        {
            Debug("");
            return base.NewDriveDynamicParameters();
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            Debug("");
            base.NewItem(path, itemTypeName, newItemValue);
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            Debug("");
            return base.NewItemDynamicParameters(path, itemTypeName, newItemValue);
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            Debug("");
            return base.NormalizeRelativePath(path, basePath);
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            Debug("");
            return base.RemoveDrive(drive);
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            Debug("");
            return base.RemoveItemDynamicParameters(path, recurse);
        }

        protected override void RenameItem(string path, string newName)
        {
            Debug("");
            base.RenameItem(path, newName);
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            Debug("");
            return base.RenameItemDynamicParameters(path, newName);
        }

        protected override void SetItem(string path, object value)
        {
            Debug("");
            base.SetItem(path, value);
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            Debug("");
            return base.SetItemDynamicParameters(path, value);
        }

        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            Debug("");
            return base.Start(providerInfo);
        }

        protected override object StartDynamicParameters()
        {
            Debug("");
            return base.StartDynamicParameters();
        }

        protected override void Stop()
        {
            Debug("");
            base.Stop();
        }

        protected override void StopProcessing()
        {
            Debug("");
            base.StopProcessing();
        }

        #endregion
    }

    public class BookmarkContentReader : IContentReader
    {
        private readonly string _path;

        public BookmarkContentReader(string path) => _path = path;

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
