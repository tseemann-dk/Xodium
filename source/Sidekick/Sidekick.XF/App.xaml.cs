using Xamarin.Forms;

namespace Sidekick.XF
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            NavigationPage = new NavigationPage(MainPage = new MainPage());
        }

        public static NavigationPage NavigationPage { get; private set; }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
