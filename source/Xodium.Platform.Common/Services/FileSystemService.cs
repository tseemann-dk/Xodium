using System.Threading.Tasks;
using System.Threading;
using Xodium.Services;
using Storage = Plugin.NetStandardStorage.Abstractions.Interfaces;

namespace Xodium.Platform.Common.Services
{
    public class FileSystemService : FileSystemServiceBase
    {
        private readonly Storage.IFileSystem fileSystem;

        public FileSystemService(Storage.IFileSystem fileSystem = null)
        {
            this.fileSystem = fileSystem ?? Plugin.NetStandardStorage.CrossStorage.FileSystem;
        }

        private IFolder localStorage;
        public override IFolder LocalStorage => localStorage ?? (localStorage = new Folder(fileSystem.LocalStorage));

        private IFolder roamingStorage;
        public override IFolder RoamingStorage => roamingStorage ?? (roamingStorage = new Folder(fileSystem.RoamingStorage));

        public override Task<IFile> GetFile(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new File(fileSystem.GetFileFromPath(path));
            return Task.FromResult<IFile>(result);
        }

        public override Task<IFolder> GetFolder(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = new Folder(fileSystem.GetFolderFromPath(path));
            return Task.FromResult<IFolder>(result);
        }
    }
}
