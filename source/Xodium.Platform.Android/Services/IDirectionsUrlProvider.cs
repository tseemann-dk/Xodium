using Xodium.Geography;

namespace Xodium.Platform.Android.Services
{
    internal interface IDirectionsUrlProvider
    {
        string GetUrl(GeoPosition position);
        string GetUrl(GeoPosition position, string address);
        string GetUrl(string address);
    }
}
