using Sidekick.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sidekick.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodeListItemView : ContentView
    {
        public NodeListItemView()
        {
            InitializeComponent();

            ImageSource.FontFamily = ResourceResolver.MaterialFontFamily;
            ImageSource.Size = 32;
            ImageSource.Color = Color.Black;
        }
    }
}