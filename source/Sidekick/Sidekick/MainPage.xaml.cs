using System.Reactive.Linq;
using Sidekick.Models;
using Sidekick.ViewModels;
using Sidekick.Views;
using Redux;
using Redux.DevTools;
using Redux.Reactive;
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
            var isDebugging = System.Diagnostics.Debugger.IsAttached;

            InitializeComponent();

            var environment = Startup.ExecutionEnvironment;
            var store = environment.GetService<IStore<AppState>>();

            BindingContext = store as TimeMachineStore<AppState>;
            TimeMachineSection.IsVisible = isDebugging;

            var projectChanges = store
                .ObserveState()
                .Select(state => state.CurrentProject)
                .DistinctUntilChanged();

            var vm = new FolderViewModel(projectChanges, environment);

            Workspace.Children.Add(new FolderView(vm));
        }
    }
}
