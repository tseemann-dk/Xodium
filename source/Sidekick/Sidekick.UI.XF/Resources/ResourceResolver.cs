using System;
using Xamarin.Forms;

namespace Sidekick.UI.XF.Resources
{
    public static class ResourceResolver
    {
        public static string MaterialFontFamily => ResolveFontName("MaterialDesignIcons.ttf", "Material Design Icons");

        public static string ResolveFontName(string fileName, string fontName)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    return $"{fileName}#{fontName}";
                case Device.iOS:
                    return $"{fontName}";
                case Device.UWP:
                    return $"Assets/Fonts/{fileName}#{fontName}";
                default:
                    throw new IndexOutOfRangeException($"Unknown platform: {Device.RuntimePlatform}");
            }
        }
    }
}
