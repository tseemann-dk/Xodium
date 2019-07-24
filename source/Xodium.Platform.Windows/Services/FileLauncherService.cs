using System.Diagnostics;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class FileLauncherService : FileLauncherServiceBase
    {
        public FileLauncherService(IFileSystemService fileSystemService) 
            : base(fileSystemService)
        {
        }

        protected override Task<bool> InternalLaunchFile(string path)
        {
            Process.Start(path);
            return Task.FromResult(true);
        }
    }
}
