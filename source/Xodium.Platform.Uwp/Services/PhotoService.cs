using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage.Pickers;
using System.IO;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class PhotoService : IPhotoService
    {
        public async Task<Photo> TakePhoto()
        {
            var dialog = new CameraCaptureUI();

            var file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (file == null) return null;

            var stream = await file.OpenReadAsync();
            return new Photo(file.Name, stream.AsStreamForRead());
        }

        public async Task<Photo> PickPhoto()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file == null) return null;

            var stream = await file.OpenReadAsync();
            return new Photo(file.Name, stream.AsStreamForRead());
        }
    }
}
