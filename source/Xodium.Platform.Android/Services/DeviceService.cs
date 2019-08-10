using Android.Content;
using Android.Runtime;
using Java.Interop;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Platform.Android.Services
{
    public class DeviceService : DeviceServiceBase
    {
        public DeviceService(Context context)
        {
            var dm = context.Resources.DisplayMetrics;
            var width = dm.WidthPixels / dm.Density;
            var height = dm.HeightPixels / dm.Density;
            var density = dm.Density;

            DisplayMetrics = new DisplayMetrics(width, height, density);
        }

        private static readonly JniPeerMembers BuildMembers = new XAPeerMembers("android/os/Build", typeof(global::Android.OS.Build));

        // NB! [TSE 2017-11-08]
        // GetSerialNumber() is a temporary workaround to https://bugzilla.xamarin.com/show_bug.cgi?id=60069
        // Use Android.OS.Build.Serial when Xamarin has fixed this issue with Android SDK 26 (Android 8.0)

        private static string GetSerialNumber()
        {
            try
            {
                const string id = "SERIAL.Ljava/lang/String;";
                var value = BuildMembers.StaticFields.GetObjectValue(id);
                return JNIEnv.GetString(value.Handle, JniHandleOwnership.TransferLocalRef);
            }
            catch
            {
                return string.Empty;
            }
        }

        public override string DeviceId => GetSerialNumber(); // TODO: => Android.OS.Build.Serial;
        public override DisplayMetrics DisplayMetrics { get; }
    }
}
