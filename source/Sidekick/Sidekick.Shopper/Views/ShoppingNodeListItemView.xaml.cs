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

            ImageSource.FontFamily = ResourceResolver.MaterialFontFamily;
            ImageSource.Size = 32;
            ImageSource.Color = Color.Black;
        }
    }
}