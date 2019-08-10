using System;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class FileSystemService : FileSystemServiceBase
    {
        public override IFolder LocalStorage { get; } = new Folder(ApplicationData.Current.LocalFolder);
        public override IFolder RoamingStorage { get; } = new Folder(ApplicationData.Current.RoamingFolder);

        public override async Task<IFile> GetFile(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return File.FromStorageFile(await StorageFile.GetFileFromPathAsync(path));
        }

        public override async Task<IFolder> GetFolder(string path, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Folder.FromStorageFolder(await StorageFolder.GetFolderFromPathAsync(path));
        }
    }
}
