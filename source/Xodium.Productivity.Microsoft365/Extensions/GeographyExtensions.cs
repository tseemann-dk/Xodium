using Microsoft.Graph;
using Xodium.Geography;

namespace Xodium.Productivity.Microsoft365.Extensions
{
    public static class GeographyExtensions
    {
        public static Location ToLocation(this GeoLocation value)
        {
            return new Location
            {
                LocationType = GetLocationType(value),
                DisplayName = value.Name,
                Coordinates = value.Position?.ToGeoCoordinates(),
                Address = value.Address?.ToPhysicalAddress()
            };
        }

        private static LocationType? GetLocationType(GeoLocation value)
        {
            if (value.Position != null)
                return LocationType.GeoCoordinates;

            if (value.Address != null)
            {
                return value.Address.PostalCode != null
                    ? LocationType.PostalAddress
                    : LocationType.StreetAddress;
            }

            return LocationType.Default;
        }

        public static OutlookGeoCoordinates ToGeoCoordinates(this GeoPosition value)
        {
            return new OutlookGeoCoordinates
            {
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Altitude = value.Altitude
            };
        }

        public static PhysicalAddress ToPhysicalAddress(this GeoAddress value)
        {
            return new PhysicalAddress
            {
                Street = value.Street,
                PostalCode = value.PostalCode,
                City = value.City,
                State = value.State,
                CountryOrRegion = value.CountryOrRegion
            };
        }

        public static GeoLocation ToGeoLocation(this Location value)
        {
            return new GeoLocation
            {
                Name = value.DisplayName,
                Position = value.Coordinates?.ToGeoPosition(),
                Address = value.Address?.ToGeoAddress()
            };
        }

        public static GeoPosition ToGeoPosition(this OutlookGeoCoordinates value)
        {
            return new GeoPosition(value.Latitude ?? 0, value.Longitude ?? 0, value.Altitude ?? 0);
        }

        public static GeoAddress ToGeoAddress(this PhysicalAddress value)
        {
            return new GeoAddress
            {
                Street = value.Street,
                PostalCode = value.PostalCode,
                City = value.City,
                State = value.State,
                CountryOrRegion = value.CountryOrRegion
            };
        }
    }
}
