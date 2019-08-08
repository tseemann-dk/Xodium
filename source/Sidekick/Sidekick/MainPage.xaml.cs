
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

            DebuggerView.IsVisible = isDebugging;
            TimeMachineSection.BindingContext = store as TimeMachineStore<AppState>;

            var projectChanges = store
                .ObserveState()
                .Select(state => state.CurrentDocument)
                .DistinctUntilChanged();

            var vm = new FolderViewModel(projectChanges, environment);

            projectChanges.Subscribe(projectState =>
            {
                StateView.Text = JsonConvert.SerializeObject(projectState, Formatting.Indented);    
            });

            Workspace.Children.Add(new FolderView(vm));
        }
    }
}
