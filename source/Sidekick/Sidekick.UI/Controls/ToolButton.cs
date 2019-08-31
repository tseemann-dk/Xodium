using Sidekick.UI.Resources;
using Xamarin.Forms;

namespace Sidekick.UI.Controls
{
    public class ToolButton : Button
    {
        private readonly FontImageSource imageSource;

        public ToolButton()
        {
            ImageSource = imageSource = new FontImageSource
            {
                FontFamily = ResourceResolver.MaterialFontFamily,
                Size = 24,
                Color = Color.Black
            };

            Padding = Device.RuntimePlatform == Device.iOS ? new Thickness(0) : new Thickness(4);
            Margin = Device.RuntimePlatform == Device.UWP ? new Thickness(4) : new Thickness(0);
        }

        public static BindableProperty GlyphProperty = BindableProperty.Create(
            nameof(Glyph), typeof(string), typeof(ToolButton), null,
            propertyChanged: (bindable, oldValue, newValue) => ((ToolButton)bindable).OnGlyphChanged());

        public string Glyph
        {
            get => GetValue(GlyphProperty) as string;
            set => SetValue(GlyphProperty, value);
        }

        private void OnGlyphChanged()
        {
            imageSource.Glyph = Glyph;
        }
    }
}
