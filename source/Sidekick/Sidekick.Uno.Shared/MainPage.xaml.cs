using System.Reactive.Linq;
using Sidekick.Shopper.ViewModels;
using Sidekick.State;
using Windows.UI.Xaml.Controls;
using Xodium.Flow;

namespace Sidekick.Uno
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            var environment = Startup.ExecutionEnvironment;
            var store = environment.GetService<IStore>() as IStore<AppState>;

            var appStateChanges = store.StateChanges;

            var shoppingSessionChanges = appStateChanges
                .Select(state => state.ShoppingSession)
                .StartWith(store.GetState().ShoppingSession)
                .DistinctUntilChanged();

            var vm = new ShoppingGroupViewModel(shoppingSessionChanges, environment);
            DataContext = vm;

            this.InitializeComponent();
        }
    }
}
