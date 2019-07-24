using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{

    public class FileSystemService : FileSystemServiceBase
    {
        private IFolder localFolder;
        private IFolder roamingFolder;

        public override IFolder LocalStorage => localFolder;
        public override IFolder RoamingStorage => roamingFolder;

        public FileSystemService()
        {
            var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            localFolder = new Folder(new DirectoryInfo(localPath));
            roamingFolder = new Folder(new DirectoryInfo(roamingPath));
        }

        public override Task<IFile> GetFile(string path, CancellationToken cancellationToken)
        {
            var fileInfo = new FileInfo(path);
            return Task.FromResult(new File(fileInfo) as IFile);
        }

        public override Task<IFolder> GetFolder(string path, CancellationToken cancellationToken)
        {
            var directoryInfo = new DirectoryInfo(path);
            return Task.FromResult(new Folder(directoryInfo) as IFolder);
        }
    }
}
