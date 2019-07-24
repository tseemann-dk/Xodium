using System.Collections.Generic;
using Xamarin.Forms;

namespace Xodium.Mvvm.Xamarin.Extensions
{
    public static class ViewExtensions
    {
        public static IEnumerable<Element> GetAllChildren(this Element element)
        {
            if (element is ContentView contentView)
            {
                yield return contentView.Content;

                foreach (var child in GetAllChildren(contentView.Content))
                {
                    yield return child;
                }
            }
            else if (element is Layout layout)
            {
                foreach (var view in layout.Children)
                {
                    yield return view;

                    foreach (var child in GetAllChildren(view))
                    {
                        yield return child;
                    }
                }
            }
        }
    }
}
