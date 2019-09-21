using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Sidekick.Shopper.UI.Uno.Views
{
    public sealed partial class ShoppingGroupView : UserControl
    {
        public ShoppingGroupView()
        {
            this.InitializeComponent();

            //DataContext = new
            //{
            //    Nodes = Enumerable.Range(1, 100).Select(x => new
            //    {
            //        Number = x.ToString(),
            //        Text = $"Node {x}"
            //    })
            //};
        }
    }
}
