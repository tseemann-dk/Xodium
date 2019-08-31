using System;
using System.Reactive.Linq;
using Newtonsoft.Json;
using Redux.DevTools;
using Sidekick.State;
using Sidekick.Shopper.ViewModels;
using Sidekick.Shopper.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xodium.Redux;
using Xodium.Flow;

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
            var store = environment.GetService<IStore>() as IStore<AppState>;

            var isDebugging =
                System.Diagnostics.Debugger.IsAttached &&
                environment.PlatformService.PlatformType == Xodium.Services.PlatformTypes.UWP;

            DebuggerView.IsVisible = isDebugging;
            RightColumn.Width = isDebugging ? GridLength.Star : new GridLength(0);

            if (store is ReduxStore<AppState> s)
            {
                TimeMachineSection.BindingContext = s.Store as TimeMachineStore<AppState>;
            }

            var appStateChanges = store.StateChanges;

            var shoppingSessionChanges = appStateChanges
                .Select(state => state.ShoppingSession)
                .StartWith(store.GetState().ShoppingSession)
                .DistinctUntilChanged();

            var vm = new ShoppingGroupViewModel(shoppingSessionChanges, environment);

            appStateChanges.Subscribe(appState => UpdateAppStateView(appState));
            //UpdateAppStateView(store.GetState());

            Workspace.Children.Add(new ShoppingGroupView(vm));
        }

        private void UpdateAppStateView(AppState appState)
        {
            AppStateView.Text = JsonConvert.SerializeObject(appState, Formatting.Indented);
        }
    }
}
