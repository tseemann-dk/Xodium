using Android.App;
using Android.Content;

namespace Xodium.Platform.Xamarin.Android.Services
{
    public delegate void ActivityResultHandler(int requestCode, Result resultCode, Intent data);

    public interface IActivitySource
    {
        Activity Activity { get; }
        event ActivityResultHandler ReceiveActivityResult;
    }
}
