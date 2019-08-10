using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class MediaPickerService : IMediaPickerService
    {
        public Task<IReadOnlyList<IMediaFile>> PickPhotos(string title)
        {
            return PickMedia(title, MediaFileType.Image, new[] { ".jpg", ".png" });
        }

        public Task<IReadOnlyList<IMediaFile>> PickVideos(string title)
        {
            return PickMedia(title, MediaFileType.Video, new[] { ".mpeg", ".avi" });
        }

        private async Task<IReadOnlyList<IMediaFile>> PickMedia(
            string title,
            MediaFileType fileType,
            IEnumerable<string> extensions)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

            foreach (var extension in extensions)
            {
                picker.FileTypeFilter.Add(extension);

            }

            var files = await picker.PickMultipleFilesAsync();

            return files?.Select(x => ToMediaFile(x, fileType)).ToList() ?? new List<IMediaFile>();
        }

        private IMediaFile ToMediaFile(StorageFile file, MediaFileType fileType)
        {
            return new MediaFile
            {
                Path = file.Path,
                Type = fileType,
                GetStream = async () =>
                {
                    var stream = await file.OpenReadAsync();
                    return stream.AsStreamForRead();
                }
            };
        }

        public class MediaFile : IMediaFile
        {
            public string Path { get; set; }
            public MediaFileType Type { get; set; }
            public Func<Task<Stream>> GetStream { get; set; }
        }
    }
}
