using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class File : IFile
    {
        private readonly StorageFile storageFile;

        public File(StorageFile storageFile)
        {
            this.storageFile = storageFile ?? throw new ArgumentNullException(nameof(storageFile));
        }

        public static File FromStorageFile(StorageFile storageFile)
        {
            return new File(storageFile);
        }

        public string Name => storageFile.Name;
        public string Path => storageFile.Path;

        public async Task<Stream> Open(FileOpenMode openMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await storageFile.OpenAsync(Map(openMode))).AsStream();
        }
        
        public async Task Delete(CancellationToken cancellationToken = default(CancellationToken))
        {
            await storageFile.DeleteAsync();
        }

        public async Task Rename(string newName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await storageFile.RenameAsync(newName);
        }

        private FileAccessMode Map(FileOpenMode openMode)
        {
            switch (openMode)
            {
                case FileOpenMode.Read:
                    return FileAccessMode.Read;
                case FileOpenMode.ReadAndWrite:
                case FileOpenMode.Write:
                    return FileAccessMode.ReadWrite;
                default:
                    throw new ArgumentOutOfRangeException(nameof(openMode), openMode, null);
            }
        }
    }
}
