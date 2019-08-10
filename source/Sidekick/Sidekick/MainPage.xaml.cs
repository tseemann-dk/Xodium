
using System;
using System.Reactive.Linq;
using Sidekick.Models;
using Sidekick.ViewModels;
using Sidekick.Views;
using Redux;
using Redux.DevTools;
using Redux.Reactive;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace Sidekick
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    [System.ComponentModel.DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var isDebugging = System.Diagnostics.Debugger.IsAttached;

            InitializeComponent();

            var environment = Startup.ExecutionEnvironment;
            var store = environment.GetService<IStore<AppState>>();

            DebuggerView.IsVisible = isDebugging && environment.PlatformService.PlatformType == Xodium.Services.PlatformTypes.UWP;
            TimeMachineSection.BindingContext = store as TimeMachineStore<AppState>;

            var appStateChanges = store.ObserveState();

            var archiveStateChanges = appStateChanges
                .Select(state => state.CurrentArchive)
                .DistinctUntilChanged();

            var vm = new FolderViewModel(archiveStateChanges, environment);

            appStateChanges.Subscribe(appState => UpdateStateView(appState));
            UpdateStateView(store.GetState());

            Workspace.Children.Add(new FolderView(vm));
        }

        private void UpdateStateView(AppState appState)
        {
            AppStateView.Text = JsonConvert.SerializeObject(appState, Formatting.Indented);
        }
    }
}
