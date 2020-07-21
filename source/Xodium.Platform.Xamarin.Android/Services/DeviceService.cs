using Android.Content;
using Android.Runtime;
using Java.Interop;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Android.Services
{
    public class DeviceService : DeviceServiceBase
    {
        public DeviceService(Context context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));

            var dm = context.Resources.DisplayMetrics;
            var width = dm.WidthPixels / dm.Density;
            var height = dm.HeightPixels / dm.Density;
            var density = dm.Density;

            DisplayMetrics = new DisplayMetrics(width, height, density);
        }

        private static readonly JniPeerMembers BuildMembers = new XAPeerMembers("android/os/Build", typeof(global::Android.OS.Build));
        private readonly Context context;

        // NB! [TSE 2017-11-08]
        // GetSerialNumber() is a temporary workaround to https://bugzilla.xamarin.com/show_bug.cgi?id=60069
        // Use Android.OS.Build.Serial when Xamarin has fixed this issue with Android SDK 26 (Android 8.0)

        //private static string GetSerialNumber()
        //{
        //    try
        //    {
        //        const string id = "SERIAL.Ljava/lang/String;";
        //        var value = BuildMembers.StaticFields.GetObjectValue(id);
        //        return JNIEnv.GetString(value.Handle, JniHandleOwnership.TransferLocalRef);
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        private string GetAndroidId() => global::Android.Provider.Settings.Secure.GetString(context.ContentResolver, global::Android.Provider.Settings.Secure.AndroidId);

        public override string DeviceId => GetAndroidId(); // Don't use GetSerialNumber() or Android.OS.Build.Serial as both are deprecated;
        public override DisplayMetrics DisplayMetrics { get; }
    }
}
