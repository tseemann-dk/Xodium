using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class PhotoService : IPhotoService
    {
        protected IMedia Media => CrossMedia.Current;

        public async Task<Photo> TakePhoto()
        {
            if (!Media.IsTakePhotoSupported)
                return null;

            var file = await Media.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full
            });

            return file == null ? null : new Photo(file.Path, file.GetStream());
        }

        public async Task<Photo> PickPhoto()
        {
            if (!Media.IsPickPhotoSupported)
                return null;

            var file = await Media.PickPhotoAsync();
            return file == null ? null : new Photo(file.Path, file.GetStream());
        }
    }
}
