using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Xodium.Services
{
    public abstract class FileSystemServiceBase : IFileSystemService
    {
        public abstract IFolder LocalStorage { get; }
        public abstract IFolder RoamingStorage { get; }

        public abstract Task<IFile> GetFile(string path, CancellationToken cancellationToken = default(CancellationToken));
        public abstract Task<IFolder> GetFolder(string path, CancellationToken cancellationToken = default(CancellationToken));

        public virtual Task<IFolder> GetTempFolder(CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetFolder(Path.GetTempPath(), cancellationToken);
        }
    }
}
