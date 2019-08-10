using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Xodium.Platform.Uwp.Extensions
{
    public static class DependencyObjectExtensions
    {
        public static T FindVisualChild<T>(this DependencyObject root, string name = null)
            where T : FrameworkElement
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                var element = child as T;

                if (element != null && (name == null || element.Name == name))
                {
                    return element;
                }

                element = FindVisualChild<T>(child, name);
                if (element != null) return element;
            }

            return null;
        }
    }
}
