using Xodium.Geography;

namespace Xodium.Utilities
{
    public interface IDirectionsUrlProvider
    {
        string GetUrl(GeoPosition position);
        string GetUrl(GeoPosition position, string address);
        string GetUrl(string address);
    }
}
