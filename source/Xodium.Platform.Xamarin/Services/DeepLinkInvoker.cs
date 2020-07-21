using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class DeepLinkInvoker : IDeepLinkInvoker
    {
        public Task<bool> CanInvoke(Uri uri)
        {
            return Launcher.CanOpenAsync(uri);
        }

        public Task Invoke(Uri uri)
        {
            return Launcher.OpenAsync(uri);
        }
    }
}
