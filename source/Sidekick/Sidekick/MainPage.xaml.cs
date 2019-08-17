using System;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Redux;
using Redux.DevTools;
using Redux.Reactive;
using Sidekick.State;
using Sidekick.Features.Shopper.ViewModels;
using Sidekick.Features.Shopper.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var environment = Startup.ExecutionEnvironment;
            var store = environment.GetService<IStore<AppState>>();

            var isDebugging =
                System.Diagnostics.Debugger.IsAttached &&
                environment.PlatformService.PlatformType == Xodium.Services.PlatformTypes.UWP;

            DebuggerView.IsVisible = isDebugging;
            RightColumn.Width = isDebugging ? GridLength.Star : new GridLength(0);
            TimeMachineSection.BindingContext = store as TimeMachineStore<AppState>;

            var appStateChanges = store.ObserveState();

            var shoppingSessionChanges = appStateChanges
                .Select(state => state.CurrentShoppingSession)
                .DistinctUntilChanged();

            var vm = new ShoppingGroupViewModel(shoppingSessionChanges, environment);

            appStateChanges.Subscribe(appState => UpdateAppStateView(appState));
            UpdateAppStateView(store.GetState());

            Workspace.Children.Add(new ShoppingGroupView(vm));
        }

        private void UpdateAppStateView(AppState appState)
        {
            AppStateView.Text = JsonConvert.SerializeObject(appState, Formatting.Indented);
        }
    }
}
