using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class File : FileSystemEntry, IFile
    {
        private readonly FileInfo fileInfo;

        public File(FileInfo fileInfo)
            : base(fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public Task<Stream> Open(FileOpenMode openMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(fileInfo.Open(FileMode.Open, Map(openMode)) as Stream);
        }

        private static FileAccess Map(FileOpenMode openMode)
        {
            switch (openMode)
            {
                case FileOpenMode.Read:
                    return FileAccess.Read;
                case FileOpenMode.Write:
                    return FileAccess.Write;
                case FileOpenMode.ReadAndWrite:
                    return FileAccess.ReadWrite;
                default:
                    throw new ArgumentOutOfRangeException(nameof(openMode));
            }
        }

        public Task Rename(string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            fileInfo.MoveTo(newName);
            return Task.CompletedTask;
        }
    }
}
