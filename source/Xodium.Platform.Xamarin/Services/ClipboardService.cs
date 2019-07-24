using Plugin.Share;
using System.Threading.Tasks;
using Xodium.Services;

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
