using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Services;
using StorageInterfaces = Plugin.NetStandardStorage.Abstractions.Interfaces;
using StorageTypes = Plugin.NetStandardStorage.Abstractions.Types;

namespace Xodium.Platform.Common.Services
{
    public class File : IFile
    {
        private StorageInterfaces.IFile file;

        public File(StorageInterfaces.IFile file)
        {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
        }

        public string Name => file.Name;
        public string Path => file.FullPath;

        public Task Delete(CancellationToken cancellationToken = default(CancellationToken))
        {
            file.Delete();
            return Task.CompletedTask;
        }

        public Task<Stream> Open(FileOpenMode openMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            var stream = file.Open(Map(openMode));
            return Task.FromResult(stream);
        }

        public Task Rename(string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            file.Rename(newName, StorageTypes.NameCollisionOption.FailIfExists);
            return Task.CompletedTask;
        }

        private FileAccess Map(FileOpenMode openMode)
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
    }
}
