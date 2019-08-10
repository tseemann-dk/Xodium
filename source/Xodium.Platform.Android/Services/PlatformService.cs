using Android.Content;
using Xodium.Platform.Xamarin.Services;

namespace Xodium.Platform.Android.Services
{
    public class PlatformService : PlatformServiceBase
    {
        private readonly Context context;

        public PlatformService(Context context)
        {
            this.context = context;
        }

        public override string AppName
        {
            get
            {
                var info = context?.ApplicationInfo;
                if (info == null) return string.Empty;
                return info.LabelRes == 0 ? info.NonLocalizedLabel.ToString() : context.GetString(info.LabelRes);
            }
        }

        public override string AppDescription
        {
            get
            {
                var info = context?.ApplicationInfo;
                if (info == null) return string.Empty;
                return info.DescriptionRes == 0 ? info.NonLocalizedLabel.ToString() : context.GetString(info.LabelRes);
            }
        }

        public override string AppVersion
        {
            get
            {
                var info = context?.PackageManager.GetPackageInfo(context.PackageName, 0);
                return info == null ? string.Empty : info.VersionName;
            }
        }

        public override string OperatingSystemName => "Android"; // TODO
        public override string OperatingSystemVersion => ""; // TODO
    }
}
