using System;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class Folder : IFolder
    {
        private readonly StorageFolder storageFolder;

        public Folder(StorageFolder storageFolder)
        {
            this.storageFolder = storageFolder ?? throw new ArgumentNullException(nameof(storageFolder));
        }

        public static Folder FromStorageFolder(StorageFolder storageFolder)
        {
            return new Folder(storageFolder);
        }

        public string Name => storageFolder.Name;
        public string Path => storageFolder.Path;

        public async Task<IFile> CreateFile(string name, FileCreateMode createMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new File(await storageFolder.CreateFileAsync(name, Map(createMode)));
        }

        private CreationCollisionOption Map(FileCreateMode createMode)
        {
            switch (createMode)
            {
                case FileCreateMode.ReplaceExisting:
                    return CreationCollisionOption.ReplaceExisting;
                case FileCreateMode.IgnoreExisting:
                    return CreationCollisionOption.OpenIfExists;
                case FileCreateMode.FailIfExists:
                    return CreationCollisionOption.FailIfExists;
                default:
                    throw new ArgumentOutOfRangeException(nameof(createMode));
            }
        }

        public async Task<IFolder> CreateFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new Folder(await storageFolder.CreateFolderAsync(name));
        }

        public async Task Delete(CancellationToken cancellationToken = default(CancellationToken))
        {
            await storageFolder.DeleteAsync();
        }

        public async Task<IFile> GetFile(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new File(await storageFolder.GetFileAsync(name));
        }

        public async Task<IReadOnlyList<IFile>> GetFiles(CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await storageFolder.GetFilesAsync()).Select(x => new File(x)).ToList();
        }

        public async Task<IFolder> GetFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new Folder(await storageFolder.GetFolderAsync(name));
        }

        public async Task<IReadOnlyList<IFolder>> GetFolders(CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await storageFolder.GetFoldersAsync()).Select(x => new Folder(x)).ToList();
        }
    }
}
