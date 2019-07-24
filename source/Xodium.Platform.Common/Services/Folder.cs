using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Services;
using StorageInterfaces = Plugin.NetStandardStorage.Abstractions.Interfaces;
using StorageTypes = Plugin.NetStandardStorage.Abstractions.Types;

namespace Xodium.Platform.Common.Services
{
    public class Folder : IFolder
    {
        private readonly StorageInterfaces.IFolder folder;

        public Folder(StorageInterfaces.IFolder folder)
        {
            this.folder = folder ?? throw new ArgumentNullException(nameof(folder));
        }

        public string Name => folder.Name;
        public string Path => folder.FullPath;

        public Task<IFile> CreateFile(string name, FileCreateMode createMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new File(folder.CreateFile(name, Map(createMode)));
            return Task.FromResult<IFile>(result);
        }

        private StorageTypes.CreationCollisionOption Map(FileCreateMode createMode)
        {
            switch (createMode)
            {
                case FileCreateMode.ReplaceExisting:
                    return StorageTypes.CreationCollisionOption.ReplaceExisting;
                case FileCreateMode.IgnoreExisting:
                    return StorageTypes.CreationCollisionOption.OpenIfExists;
                case FileCreateMode.FailIfExists:
                    return StorageTypes.CreationCollisionOption.FailIfExists;
                default:
                    throw new ArgumentOutOfRangeException(nameof(createMode));
            }
        }

        public Task<IFolder> CreateFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new Folder(folder.CreateFolder(name, StorageTypes.CreationCollisionOption.FailIfExists));
            return Task.FromResult<IFolder>(result);
        }

        public Task Delete(CancellationToken cancellationToken = default(CancellationToken))
        {
            folder.Delete();
            return Task.CompletedTask;
        }

        public Task<IFile> GetFile(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new File(folder.GetFile(name));
            return Task.FromResult<IFile>(result);
        }

        public Task<IReadOnlyList<IFile>> GetFiles(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = folder.GetFiles().Select(x => new File(x)).ToList();
            return Task.FromResult<IReadOnlyList<IFile>>(result) ;
        }

        public Task<IFolder> GetFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new Folder(folder.GetFolder(name));
            return Task.FromResult<IFolder>(result);
        }

        public Task<IReadOnlyList<IFolder>> GetFolders(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = folder.GetFolders().Select(x => new Folder(x)).ToList();
            return Task.FromResult<IReadOnlyList<IFolder>>(result);
        }
    }
}
