using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class FileLauncherService : FileLauncherServiceBase
    {
        public FileLauncherService(IFileSystemService fileSystemService = null)
            : base(fileSystemService)
        {
        }

        protected override Task<bool> InternalLaunchFile(string path)
        {
            var uri = new Uri("file://" + path);
            Device.OpenUri(uri);
            return Task.FromResult(true);
        }
    }
}
