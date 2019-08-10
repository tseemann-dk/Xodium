using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Platform.Android.Services
{
    public class MediaPickerService : IMediaPickerService
    {
        private readonly IActivitySource activitySource;
        private readonly int pickRequestCode;
        private TaskCompletionSource<IReadOnlyList<IMediaFile>> completionSource;

        public MediaPickerService(IActivitySource activitySource, int? pickRequestCode = null)
        {
            this.activitySource = activitySource ?? throw new ArgumentNullException(nameof(activitySource));
            this.pickRequestCode = pickRequestCode ?? 9001;
        }

        private Activity Activity => activitySource.Activity;

        public void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            activitySource.ReceiveActivityResult -= OnActivityResult;

            var mediaFiles = new List<IMediaFile>();

            try
            {
                if (requestCode == pickRequestCode && resultCode == Result.Ok && data != null)
                {
                    var clipData = data.ClipData;

                    if (clipData != null)
                    {
                        for (int i = 0; i < clipData.ItemCount; i++)
                        {
                            mediaFiles.Add(CreateMediaFileFromUri(clipData.GetItemAt(i).Uri));
                        }
                    }
                    else
                    {
                        mediaFiles.Add(CreateMediaFileFromUri(data.Data));
                    }
                }
            }
            finally
            {
                completionSource?.TrySetResult(mediaFiles);
            }
        }

        IMediaFile CreateMediaFileFromUri(global::Android.Net.Uri uri)
        {
            return new MediaFile
            {
                Path = uri.Path,
                Type = MediaContentTypes.ToFileType(Activity.ContentResolver.GetType(uri)),
                GetStream = () =>
                {
                    var bitmap = MediaStore.Images.Media.GetBitmap(Activity.ContentResolver, uri);
                    var stream = new MemoryStream();
                    bitmap.Compress(global::Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                    return Task.FromResult<Stream>(stream);
                }
            };
        }

        public Task<IReadOnlyList<IMediaFile>> PickPhotos(string title)
        {
            return PickMedia("image/*", title, pickRequestCode);
        }

        public Task<IReadOnlyList<IMediaFile>> PickVideos(string title)
        {
            return PickMedia("video/*", title, pickRequestCode);
        }

        private async Task<IReadOnlyList<IMediaFile>> PickMedia(string type, string title, int requestCode)
        {
            completionSource = new TaskCompletionSource<IReadOnlyList<IMediaFile>>();

            var intent = new Intent(Intent.ActionPick)
                .SetType(type)
                .PutExtra(Intent.ExtraAllowMultiple, true);

            activitySource.ReceiveActivityResult += OnActivityResult;
            Activity.StartActivityForResult(Intent.CreateChooser(intent, title), requestCode);
            return await completionSource.Task;
        }
    }
}