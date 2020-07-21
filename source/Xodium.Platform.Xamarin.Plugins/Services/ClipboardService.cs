using Plugin.Share;
using Xodium.Services;
using System.Threading.Tasks;

namespace Xodium.Platform.Xamarin.Services
{
    public class ClipboardService : IClipboardService
    {
        public bool IsClipboardSupported => CrossShare.Current.SupportsClipboard;

        public Task CopyText(string text)
        {
            return CrossShare.Current.SetClipboardText(text);
        }
    }
}
