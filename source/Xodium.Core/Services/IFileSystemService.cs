using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IFileSystemEntry
    {
        string Name { get; }
        string Path { get; }
    }

    public interface IFileSystemService
    {
        IFolder LocalStorage { get; }
        IFolder RoamingStorage { get; }

        Task<IFile> GetFile(string path, CancellationToken cancellationToken = default);
        Task<IFolder> GetFolder(string path, CancellationToken cancellationToken = default);
        Task<IFolder> GetTempFolder(CancellationToken cancellationToken = default);
    }
}
