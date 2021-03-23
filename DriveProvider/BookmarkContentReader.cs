using System.Collections;
using System.IO;
using System.Linq;
using System.Management.Automation.Provider;

namespace DriveProvider
{
    public class BookmarkContentReader : IContentReader
    {
        private readonly string _path;
        private long pos;

        public BookmarkContentReader(string path) => _path = path;

        public void Dispose()
        {
        }

        public void Close()
        {
        }

        public IList Read(long readCount)
        {
            //some operations such as get-content may make repeated calls
            //and it appears the way to signal that we are at the end of the
            //stream is to return an empty list
            if (pos == 0)
            {
                pos++;
                return new[] {_path}.ToList();
            }

            return new string[0].ToList();
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    pos = offset;
                    break;
                case SeekOrigin.Current:
                    pos += offset;
                    break;
                case SeekOrigin.End:
                    pos -= offset;
                    break;
            }
        }
    }
}
