using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Platform.Windows.Services
{
    public class FileSystemEntry
    {
        protected FileSystemInfo Info { get; }

        public FileSystemEntry(FileSystemInfo info)
        {
            Info = info;
        }

        public string Name => Info.Name;
        public string Path => Info.FullName;

        public Task Delete(CancellationToken cancellationToken = default(CancellationToken))
        {
            Info.Delete();
            return Task.CompletedTask;
        }
    }
}
