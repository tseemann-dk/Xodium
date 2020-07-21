using System;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Android.Services
{
    public class MediaContentTypes
    {
        public const string Image = "image";
        public const string Video = "video";

        public static MediaFileType ToFileType(string contentType)
        {
            var parts = contentType.ToLower().Split('/');

            if (parts.Length <= 1)
                throw new ArgumentException("Invalid content type", nameof(contentType));

            switch (parts[0])
            {
                case Image:
                    return MediaFileType.Image;
                case Video:
                    return MediaFileType.Video;
                default:
                    throw new IndexOutOfRangeException(contentType);
            }
        }
    }
}