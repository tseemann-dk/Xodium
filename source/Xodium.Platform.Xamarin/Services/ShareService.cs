using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Threading.Tasks;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class ShareService : IShareService
    {
        public Task ShareLink(Uri uri, string title)
        {
            var message = new ShareMessage
            {
                Url = uri.ToString(),
                Title = title
            };

            return CrossShare.Current.Share(message);
        }

        public Task ShareText(string text, string title)
        {
            var message = new ShareMessage
            {
                Text = text,
                Title = title
            };

            return CrossShare.Current.Share(message);
        }
    }
}
