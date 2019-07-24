namespace Xodium.Services
{
    public class DisplayMetrics
    {
        public DisplayMetrics(double width, double height, double scaleFactor)
        {
            Width = width;
            Height = height;
            ScaleFactor = scaleFactor;
        }

        public double Height { get; }
        public double Width { get; }
        public double ScaleFactor { get; }
    }
}
