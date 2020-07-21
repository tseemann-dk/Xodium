namespace Xodium.Platform.Xamarin.Services
{
    public class FileSystemService : Common.Services.FileSystemService
    {
        public FileSystemService() 
            : base(Plugin.NetStandardStorage.CrossStorage.FileSystem)
        {
        }
    }
}
