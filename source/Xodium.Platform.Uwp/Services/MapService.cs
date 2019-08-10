using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.System;
using Xodium.Geography;
using Xodium.Services;
using Xodium.Utilities;

namespace Xodium.Platform.Uwp.Services
{
    public class MapService : IMapService
    {
        class DirectionsUrlProvider : IDirectionsUrlProvider
        {
            const string BaseUrlFormat = "bingmaps:?rtp=~{0}";
            const string AddressUrlFormat = BaseUrlFormat;
            const string PositionUrlFormat = BaseUrlFormat;
            const string PositionAndAddressUrlFormat = BaseUrlFormat;

            public string GetUrl(GeoPosition position)
                => string.Format(PositionUrlFormat, ToPositionSpecifier(position));

            public string GetUrl(GeoPosition position, string address)
                => string.Format(PositionAndAddressUrlFormat, $"{ToPositionSpecifier(position)}_{GetAddress(address)}");

            public string GetUrl(string address)
                => string.Format(AddressUrlFormat, ToAddressSpecifier(address));
        }

        private static DirectionsUrlProvider directionsUrlProvider = new DirectionsUrlProvider();

        public Task<string> GetAddressFromPosition(GeoPosition position)
        {
            throw new NotImplementedException();
        }

        public Task<GeoPosition> GetPositionFromAddress(string address)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ProvideDirections(GeoPosition destination, string destinationName = null, GeoPosition source = null, string sourceName = null)
        {
            var url = GetDirectionsUrl(destination, destinationName);

            if (string.IsNullOrEmpty(url)) return false;

            //var src = BuildLocationParameter(source, sourceName);
            //var dst = BuildLocationParameter(destination, destinationName);
            //var uri = new Uri($"bingmaps:?rtp={src}~{dst}");

            return await Launcher.LaunchUriAsync(new Uri(url));
        }

        // Used for test purposes
        public static string GetDirectionsUrl(GeoPosition destination, string destinationName = null)
        {
            return new DirectionsUrlBuilder(directionsUrlProvider).Build(destination, destinationName);
        }

        private static string BuildLocationParameter(GeoPosition position, string name)
        {
            if (position == null)
                return string.Empty;

            if (position.IsEmpty)
                return ToAddressSpecifier(name);

            var result = ToPositionSpecifier(position);

            if (name != null)
            {
                result += "_" + Uri.EscapeUriString(name);
            }

            return result;
        }

        private static string ToAddressSpecifier(string address)
        {
            return string.IsNullOrEmpty(address) ? string.Empty : $"adr.{GetAddress(address)}";
        }

        private static string GetAddress(string address)
        {
            return string.IsNullOrEmpty(address) ? string.Empty : Uri.EscapeUriString(address);
        }

        private static string ToPositionSpecifier(GeoPosition position)
        {
            var culture = CultureInfo.InvariantCulture;
            var lat = position.Latitude.ToString(culture);
            var lon = position.Longitude.ToString(culture);

            return $"pos.{lat}_{lon}";
        }
    }
}
