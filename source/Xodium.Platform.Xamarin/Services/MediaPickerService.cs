using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class MediaPickerService : IMediaPickerService
    {
        private readonly IPhotoService photoService;

        public MediaPickerService(IPhotoService photoService)
        {
            this.photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        public async Task<IReadOnlyList<IMediaFile>> PickPhotos(string title)
        {
            var photo = await photoService.PickPhoto();

            var files = new List<IMediaFile>();

            if (photo != null)
            {
                files.Add(new MediaFile
                {
                    Path = photo.Name,
                    Type = MediaFileType.Image,
                    GetStream = () => Task.FromResult(photo.Stream)
                });
            }

            return files;
        }

        public Task<IReadOnlyList<IMediaFile>> PickVideos(string title)
        {
            throw new NotImplementedException();
        }
    }
}
