using System;
using Redux;
using Redux.DevTools;
using Redux.DevTools.Universal;
using Sidekick.Models;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Sidekick.UWP
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var isDebugging = System.Diagnostics.Debugger.IsAttached;

            if (!(Window.Current.Content is Frame rootFrame))
            {
                Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
                Xamarin.Forms.Forms.Init(e);

                var bootstrapper = new UwpBootstrapper((reducer, state) => isDebugging
                    ? new TimeMachineStore<AppState>(reducer, state) as IStore<AppState>
                    : new Store<AppState>(reducer, state));

                Startup.Init(bootstrapper);

                rootFrame = isDebugging
                    ? new DevFrame { TimeMachineStore = (IStore<TimeMachineState>)bootstrapper.Store }
                    : new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            Window.Current.Activate();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
