using Foundation;
using UIKit;

namespace Sidekick.XF.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init();
            Rg.Plugins.Popup.Popup.Init();

            Startup.Init(new iOSBootstrapper());
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
