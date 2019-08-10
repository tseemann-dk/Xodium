using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class FileLauncherService : IFileLauncherService
    {
        public async Task<bool> LaunchFile(string path)
        {
            return await LaunchFile(await StorageFile.GetFileFromPathAsync(path));
        }

        public async Task<string> LaunchFile(Stream stream, string name)
        {
            var folder = ApplicationData.Current.TemporaryFolder;
            var file = await folder.CreateFileAsync(name, CreationCollisionOption.GenerateUniqueName);

            using (var output = await file.OpenStreamForWriteAsync())
            {
                await stream.CopyToAsync(output);
            }

            return await LaunchFile(file) ? file.Path : null;
        }

        private async Task<bool> LaunchFile(IStorageFile file)
        {
            if (file == null) return false;

            var options = new LauncherOptions
            {
                DisplayApplicationPicker = false,
                 
            };

            return await Launcher.LaunchFileAsync(file, options);
        }
    }
}
