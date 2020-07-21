using System;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using UIKit;
using Xodium.Geography;
using Xodium.Services;

namespace Xodium.Platform.iOS.Services
{
    public class MapService : IMapService
    {
        public Task<string> GetAddressFromPosition(GeoPosition position)
        {
            // TODO
            return Task.FromResult(string.Empty);
        }

        public async Task<GeoPosition> GetPositionFromAddress(string address)
        {
            try
            {
                var coder = new CLGeocoder();
                var result = await coder.GeocodeAddressAsync(address);
                var place = result.FirstOrDefault();
                return place == null ? null : new GeoPosition(place.Location.Coordinate.Latitude, place.Location.Coordinate.Longitude);
            }
            catch (Exception)
            {
                return new GeoPosition();
            }
        }

        public Task<bool> ProvideDirections(GeoPosition destination, string destinationName, GeoPosition source, string sourceName)
        {
            const string mapsPrefix = "maps.apple.com";

            var address = $"http://{mapsPrefix}/?";

            address +=
                FormatLocation("daddr", destination) + "&" +
                FormatLocation("saddr", source);

            return Task.FromResult(UIApplication.SharedApplication.OpenUrl(new NSUrl(address)));
        }

        private static string FormatLocation(string prefix, GeoPosition position)
        {
            return $"{prefix}={position}";
        }
    }
}
