using Sidekick.UI.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick.Shopper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingNodeListItemView : ContentView
    {
        public ShoppingNodeListItemView()
        {
            InitializeComponent();

            GroupImageSource.FontFamily = ResourceResolver.MaterialFontFamily;
            GroupImageSource.Size = 60;
            GroupImageSource.Color = Color.Black;
        }
    }
}