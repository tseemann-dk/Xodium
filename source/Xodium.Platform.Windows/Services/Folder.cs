using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class Folder : FileSystemEntry, IFolder
    {
        private readonly DirectoryInfo directoryInfo;

        public Folder(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        public Task<IFile> CreateFile(string name, FileCreateMode createMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            var filePath = System.IO.Path.Combine(Path, name);
            var fileInfo = new FileInfo(filePath);
            var stream = fileInfo.Create();
            stream.Close();
            return Task.FromResult(new File(fileInfo) as IFile);
        }

        public Task<IFolder> CreateFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var subdirectory = directoryInfo.CreateSubdirectory(name);
            return Task.FromResult(new Folder(subdirectory) as IFolder);
        }

        public Task<IFile> GetFile(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var fileInfos = directoryInfo.GetFiles(name);
            var fileInfo = fileInfos.FirstOrDefault();
            var file = fileInfo == null ? null : new File(fileInfo);
            return Task.FromResult((IFile) file);
        }

        public Task<IReadOnlyList<IFile>> GetFiles(CancellationToken cancellationToken = default(CancellationToken))
        {
            var files = new List<IFile>();

            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                files.Add(new File(fileInfo));
            }

            return Task.FromResult((IReadOnlyList<IFile>) files);
        }

        public Task<IFolder> GetFolder(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var subdirectories = directoryInfo.GetDirectories(name);
            var subdirectory = subdirectories.FirstOrDefault();
            var folder = subdirectory == null ? null : new Folder(subdirectory);
            return Task.FromResult((IFolder) folder);
        }

        public Task<IReadOnlyList<IFolder>> GetFolders(CancellationToken cancellationToken = default(CancellationToken))
        {
            var folders = directoryInfo
                .EnumerateDirectories()
                .Select(subdirectory => new Folder(subdirectory))
                .Cast<IFolder>().ToList();

            return Task.FromResult((IReadOnlyList<IFolder>) folders);
        }
    }
}
