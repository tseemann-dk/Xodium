using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class FileLauncherService : FileLauncherServiceBase
    {
        public FileLauncherService(IFileSystemService fileSystemService = null)
            : base(fileSystemService)
        {
        }

        protected override async Task<bool> InternalLaunchFile(string path)
        {
            var uri = new Uri("file://" + path);

            if (await Launcher.CanOpenAsync(uri))
            {
                await Launcher.OpenAsync(uri);
                return true;
            }

            return false;
        }
    }
}
