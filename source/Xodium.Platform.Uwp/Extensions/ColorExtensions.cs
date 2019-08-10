using Windows.UI;

namespace Xodium.Platform.Uwp.Extensions
{
    public static class ColorExtensions
    {
        public static Color AdjustBrightness(this Color color, double factor)
        {
            return Color.FromArgb(color.A, 
                (byte)(color.R*factor), 
                (byte)(color.G*factor), 
                (byte)(color.B*factor));
        }
    }
}
