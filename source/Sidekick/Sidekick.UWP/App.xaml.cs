using System;
using Redux;
using Redux.DevTools;
using Redux.DevTools.Universal;
using Sidekick.State;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xodium.Redux;

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
                Rg.Plugins.Popup.Popup.Init();

                var bootstrapper = new UwpBootstrapper((reducer, state, middlewares) => 
                    new ReduxStore<AppState>(
                        r => isDebugging
                            ? new TimeMachineStore<AppState>(r, state, middlewares) as IStore<AppState>
                            : new Store<AppState>(r, state, middlewares),
                        reducer
                    ));

                Startup.Init(bootstrapper);

                rootFrame = isDebugging && bootstrapper.Store is ReduxStore<AppState> rs
                    ? new DevFrame { TimeMachineStore = (IStore<TimeMachineState>)rs.Store }
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
