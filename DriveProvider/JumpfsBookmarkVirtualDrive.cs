using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Runtime.CompilerServices;
using Core.Bookmarking;
using Core.EnvironmentAccess;
using Core.Extensions;

namespace DriveProvider
{
    [CmdletProvider("jumpfs", ProviderCapabilities.ExpandWildcards)]
    public class JumpfsBookmarkVirtualDrive : NavigationCmdletProvider, IContentCmdletProvider
    {
        //TODO - possibly this could be cached - it's a little unclear what the lifecycle of this class is
        //By NOT caching we can,at least, avoid problems with cross-over reads and writes from different sessions
        private BookmarkRepository Repo => new BookmarkRepository(new JumpfsEnvironment());


        protected override string[] ExpandPath(string path)
        {
            return DebugR(path, () =>
            {
                path ??= "";
                path = TranslatePath(path);
                var bmk = Repo.Load();
                return BookmarkRepository.Match(bmk, path)
                    .Select(b => b.Name).ToArray();
            });
        }

        protected override bool HasChildItems(string path)
        {
            return DebugR(path,
                () => false);
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            DebugR(path, () => Repo.Remove(TranslatePath(path)));
        }


        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            DebugR(path, () =>
            {
                var bookmarks = Repo.Load()
                    .OrderBy(b => b.Name)
                    .Select(b => b.Name)
                    .ToArray();
                foreach (var b in bookmarks)
                    WriteItemObject(b, path, false);
            });
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            DebugR($"{path} {recurse}",
                () =>
                {
                    var bookmarks = Repo.Load()
                        .OrderBy(b => b.Name);
                    foreach (var b in bookmarks)
                    {
                        WriteItemObject(b, path, false);
                    }
                });
        }

        //we are an item container if at the root
        protected override bool IsItemContainer(string path)
        {
            return DebugR($"{path}", () => TranslatePath(path) == "");
        }


        //just say that all paths are valid
        protected override bool IsValidPath(string path) => true;

        private string TranslatePath(string path) => path == null ? "*" : path.Replace(PSDriveInfo.Root, "");

        protected override bool ItemExists(string path)
        {
            return DebugR(path,
                () =>
                {
                    if (path == null)
                        return true;

                    path = TranslatePath(path);
                    var bookmarks = Repo.Load();
                    return string.IsNullOrEmpty(path) || bookmarks.Any(b => b.Name == path);
                });
        }


        //for testing, just return a string
        protected override void GetItem(string path)
        {
            DebugR(path,
                () =>
                {
                    var bookmarks = Repo.Load();
                    foreach (var bookmark in bookmarks.Where(b => b.Name == TranslatePath(path)))
                    {
                        WriteItemObject(bookmark, path, false);
                    }
                });
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            return DebugR("", () => base.NewDrive(drive));
        }


        #region CONTENT

        public IContentReader GetContentReader(string path)
        {
            return DebugR(path,
                () =>
                {
                    var bookmarks = Repo.Load();
                    path = TranslatePath(path);
                    if (bookmarks.TryGetSingle(b => b.Name == path, out var hit))
                    {
                        return new BookmarkContentReader(hit.Path);
                    }

                    throw new ArgumentException($"{path} is not a valid bookmark name");
                });
        }


        public object GetContentReaderDynamicParameters(string path)
        {
            return DebugR<object>(path, () => null);
        }

        #region CONTENT notimplemented

        public void ClearContent(string path)
        {
            DebugR(path, () => throw new NotImplementedException());
        }

        public object ClearContentDynamicParameters(string path)
        {
            return DebugR<object>(path, () => throw new NotImplementedException());
        }

        public IContentWriter GetContentWriter(string path)
        {
            return DebugR<IContentWriter>(path, () => throw new NotImplementedException());
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            return DebugR<object>(path, () => throw new NotImplementedException());
        }

        #endregion CONTENT notimplemented

        #endregion CONTENT

        #region tracing - no overridden code here

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            return DebugR($"p:{path} r:{recurse}", () => base.GetChildItemsDynamicParameters(path, recurse));
        }

        protected override void ClearItem(string path)
        {
            DebugR("", () =>
                base.ClearItem(path));
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            return DebugR("", () => base.ClearItemDynamicParameters(path));
        }

        protected override bool ConvertPath(string path, string filter, ref string updatedPath,
            ref string updatedFilter)
        {
            Debug("");
            _debugLevel++;
            var r = base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
            _debugLevel--;
            Debug($"ret:{r}");
            return r;
        }


        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            DebugR("", () => base.CopyItem(path, copyPath, recurse));
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            return DebugR("", () => base.CopyItemDynamicParameters(path, destination, recurse));
        }

        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            DebugR("",
                () => base.GetChildItems(path, recurse, depth));
        }

        protected override string GetChildName(string path)
        {
            return DebugR(path, () => base.GetChildName(path));
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return DebugR(path, () => base.GetChildNamesDynamicParameters(path));
        }

        protected override object GetItemDynamicParameters(string path)
        {
            return DebugR(path, () => base.GetItemDynamicParameters(path));
        }

        protected override string GetParentPath(string path, string root)
        {
            return DebugR($"p:{path} r:{root}", () => base.GetParentPath(path, root));
        }

        public override string GetResourceString(string baseName, string resourceId)
        {
            return DebugR("", () => base.GetResourceString(baseName, resourceId));
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            return DebugR("", () => base.InitializeDefaultDrives());
        }

        protected override void InvokeDefaultAction(string path)
        {
            DebugR("", () => base.InvokeDefaultAction(path));
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return DebugR("", () => base.InvokeDefaultActionDynamicParameters(path));
        }


        protected override object ItemExistsDynamicParameters(string path)
        {
            return DebugR("", () => base.ItemExistsDynamicParameters(path));
        }


        protected override string MakePath(string parent, string child)
        {
            return DebugR($"p:{parent} c:{child}", () => base.MakePath(parent, child));
        }

        protected override void MoveItem(string path, string destination)
        {
            DebugR("", () => base.MoveItem(path, destination));
        }

        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return DebugR("", () => base.MoveItemDynamicParameters(path, destination));
        }

        protected override object NewDriveDynamicParameters()
        {
            return DebugR("", () => base.NewDriveDynamicParameters());
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            DebugR("", () => base.NewItem(path, itemTypeName, newItemValue));
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return DebugR("", () => base.NewItemDynamicParameters(path, itemTypeName, newItemValue));
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return DebugR("", () => base.NormalizeRelativePath(path, basePath));
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            return DebugR("", () => base.RemoveDrive(drive));
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return DebugR("", () => base.RemoveItemDynamicParameters(path, recurse));
        }

        protected override void RenameItem(string path, string newName)
        {
            DebugR("", () => base.RenameItem(path, newName));
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return DebugR("", () => base.RenameItemDynamicParameters(path, newName));
        }

        protected override void SetItem(string path, object value)
        {
            DebugR("", () => base.SetItem(path, value));
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return DebugR("", () => base.SetItemDynamicParameters(path, value));
        }

        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            return DebugR("", () => base.Start(providerInfo));
        }

        protected override object StartDynamicParameters()
        {
            return DebugR("", () => base.StartDynamicParameters());
        }

        protected override void Stop()
        {
            DebugR("", () => base.Stop());
        }

        protected override void StopProcessing()
        {
            DebugR("", () => base.StopProcessing());
        }

        #endregion

        #region debug

        private int _debugLevel;

        private void DebugIndented(string msg)
        {
            var pad = "".PadRight(_debugLevel * 4);
            WriteDebug($"{pad}{msg}");
        }


        private void Debug(string message, [CallerMemberName] string caller = "none")
        {
            DebugIndented($"{caller}:  {message}");
        }

        private T DebugR<T>(string message, Func<T> act, [CallerMemberName] string caller = "none")
        {
            DebugIndented($"{caller}:  {message}");
            _debugLevel++;
            var r = act();
            _debugLevel--;
            DebugIndented($"{caller}:  returned {r}");
            return r;
        }

        private void DebugR(string message, Action act, [CallerMemberName] string caller = "none")
        {
            DebugIndented($"{caller}:  {message}");
            _debugLevel++;
            act();
            _debugLevel--;
            DebugIndented($"{caller}:  returned (void)");
        }

        #endregion debug
    }
}
