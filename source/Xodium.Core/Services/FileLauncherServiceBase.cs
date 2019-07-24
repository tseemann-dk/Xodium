using System;
using System.IO;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public abstract class FileLauncherServiceBase : IFileLauncherService
    {
        protected FileLauncherServiceBase(IFileSystemService fileSystemService)
        {
            FileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        protected IFileSystemService FileSystemService { get; }

        protected abstract Task<bool> InternalLaunchFile(string path);

        public virtual Task<bool> LaunchFile(string path)
        {
            return InternalLaunchFile(path);
        }

        public virtual async Task<string> LaunchFile(Stream stream, string fileName)
        {
            var folder = await FileSystemService.GetTempFolder();
            if (folder == null) throw new InvalidOperationException("Temporary folder not available");

            var file = await folder.CreateFile(fileName, FileCreateMode.ReplaceExisting);

            using (var output = await file.Open(FileOpenMode.Write))
            {
                stream.Position = 0;
                await stream.CopyToAsync(output);
            }

            return await InternalLaunchFile(file.Path) ? file.Path : null;
        }
    }
}
