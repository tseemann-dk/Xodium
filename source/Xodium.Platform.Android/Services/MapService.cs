using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Geography;
using Xodium.Services;

namespace Xodium.Platform.Android.Services
{
    public class MapService : IMapService
    {
        class DirectionsUrlProvider : IDirectionsUrlProvider
        {
            const string AddressUrlFormat = "https://www.google.com/maps/dir/?api=1&destination={0}";
            const string PositionUrlFormat = "https://www.google.com/maps/dir/?api=1&destination={0}";
            const string PositionAndAddressUrlFormat = "https://www.google.com/maps/dir/Current+Location/{1}/@{0},15z";

            public string GetUrl(GeoPosition position, string address)
                => GetUrl(position); // Google.Maps will make a guess based on address - use exact position instead
                //=> string.Format(PositionAndAddressUrlFormat, position, string.IsNullOrEmpty(address) ? string.Empty : Uri.EscapeUriString(address));

            public string GetUrl(string address)
                => string.Format(AddressUrlFormat, string.IsNullOrEmpty(address) ? string.Empty : Uri.EscapeUriString(address));

            public string GetUrl(GeoPosition position)
                => string.Format(PositionUrlFormat, position);
        }

        private static DirectionsUrlProvider directionsUrlProvider = new DirectionsUrlProvider();

        public Task<string> GetAddressFromPosition(GeoPosition position)
        {
            // TODO
            return Task.FromResult(string.Empty);
        }

        public Task<GeoPosition> GetPositionFromAddress(string address)
        {
            // TODO
            return Task.FromResult(new GeoPosition(0, 0));
        }
      
        public Task<bool> ProvideDirections(GeoPosition destination, string destinationName, GeoPosition source, string sourceName)
        {
            var url = GetDirectionsUrl(destination, destinationName);

            if (string.IsNullOrEmpty(url))
                return Task.FromResult(false);

            Device.OpenUri(new Uri(url));

            return Task.FromResult(true);
        }

        private static string GetDirectionsUrl(GeoPosition destination, string destinationName = null)
        {
            return new DirectionsUrlBuilder(directionsUrlProvider).Build(destination, destinationName);
        }
    }
}
