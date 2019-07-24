using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public enum FileCreateMode { ReplaceExisting, IgnoreExisting, FailIfExists }

    public interface IFolder : IFileSystemEntry
    {
        Task<IFile> CreateFile(string name, FileCreateMode createMode, CancellationToken cancellationToken = default);
        Task<IFolder> CreateFolder(string name, CancellationToken cancellationToken = default);
        Task Delete(CancellationToken cancellationToken = default(CancellationToken));
        Task<IFile> GetFile(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IFile>> GetFiles(CancellationToken cancellationToken = default);
        Task<IFolder> GetFolder(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IFolder>> GetFolders(CancellationToken cancellationToken = default);
    }
}
