using ReactiveUI.XamForms;
using Sidekick.Features.Shopper.ViewModels;
using Xamarin.Forms.Xaml;

namespace Sidekick.Features.Shopper.Views
{
    public class ShopVisitViewBase : ReactiveContentView<ShopVisitViewModel> { }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopVisitView : ShopVisitViewBase
    {
        public ShopVisitView()
        {
            InitializeComponent();
        }
    }
}