using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation.Provider;

namespace DriveProvider
{
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
