using Sidekick.UI.XF.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick.Shopper.UI.XF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingNodeListItemView : ContentView
    {
        public ShoppingNodeListItemView()
        {
            InitializeComponent();

            FolderImageSource.FontFamily = ResourceResolver.MaterialFontFamily;
            FolderImageSource.Size = 60;
            FolderImageSource.Color = Color.Black;
        }
    }
}