using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public enum FileOpenMode { Read, Write, ReadAndWrite }

    public interface IFile : IFileSystemEntry
    {
        Task<Stream> Open(FileOpenMode openMode, CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(CancellationToken cancellationToken = default(CancellationToken));
        Task Rename(string newName, CancellationToken cancellationToken = default(CancellationToken));
    }

    public static class FileExtensions
    {
        public static Uri ToUri(this IFile file)
        {
            return new Uri(file.Path);
        }
    }
}
