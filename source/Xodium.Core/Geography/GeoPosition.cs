using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xodium.Geography
{
    public class GeoPosition
    {
        public GeoPosition()
        {
        }

        public GeoPosition(double latitude, double longitude, double altitude = 0.0)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
        public double Altitude { get; }

        public bool HasAltitude => Altitude < 0 || Altitude > 0;

        public static GeoPosition Empty = new GeoPosition();

        #region Conversion

        private static double ToRadians(double degrees) => degrees * (Math.PI / 180);
        private static double ToDegrees(double radians) => radians * 180 / Math.PI;

        public static GeoPosition Parse(string value)
        {
            var values = value.Split(',');
            return values.Length < 2 ? null : new GeoPosition(DoubleFromString(values[0]), DoubleFromString(values[1]));
        }

        public override string ToString()
        {
            var pos = $"{DoubleToString(Latitude)},{DoubleToString(Longitude)}";
            return HasAltitude ? pos + "," + DoubleToString(Altitude) : pos;
        }

        private static string DoubleToString(double value)
        {
            return value.ToString("G", CultureInfo.InvariantCulture);
        }

        private static double DoubleFromString(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        #endregion

        #region Distance & Bearing

        public const double EarthRadiusInKilometers = 6367.0;

        private static double RadianDelta(double degrees1, double degrees2) { return ToRadians(degrees2) - ToRadians(degrees1); }
        private static double ToBearing(double radians) => (ToDegrees(radians) + 360) % 360;

        public GeoDistance DistanceFrom(GeoPosition other)
        {
            var kilometers = EarthRadiusInKilometers * 2 * Math.Asin(
                Math.Min(1,
                    Math.Sqrt(Math.Pow(Math.Sin(RadianDelta(Latitude, other.Latitude) / 2.0), 2.0) +
                    Math.Cos(ToRadians(Latitude)) * Math.Cos(ToRadians(other.Latitude)) *
                    Math.Pow(Math.Sin((RadianDelta(Longitude, other.Longitude)) / 2.0), 2.0))
                )
            );

            return GeoDistance.FromKilometers(kilometers);
        }

        public double BearingTo(GeoPosition other)
        {
            var dLon = ToRadians(other.Longitude - Longitude);
            var dPhi = Math.Log(Math.Tan(ToRadians(other.Latitude) / 2 + Math.PI / 4) / Math.Tan(ToRadians(Latitude) / 2 + Math.PI / 4));
            if (Math.Abs(dLon) > Math.PI)
                dLon = dLon > 0 ? -(2 * Math.PI - dLon) : 2 * Math.PI + dLon;
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        #endregion

        #region Comparison

        public override bool Equals(object obj)
        {
            if (!(obj is GeoPosition))
                return false;

            var other = (GeoPosition)obj;

            return other.Altitude.Equals(Altitude)
                && other.Latitude.Equals(Latitude)
                && other.Longitude.Equals(Longitude);
        }

        public override int GetHashCode()
        {
            return Altitude.GetHashCode() ^ Longitude.GetHashCode() ^ Latitude.GetHashCode();
        }

        public static bool IsNullOrEmpty(GeoPosition value) => value == null || value.IsEmpty;

        public bool IsEmpty => IsZero(Latitude) && IsZero(Longitude) && IsZero(Altitude);

        private static bool IsZero(double value)
        {
            return Math.Abs(value) < double.Epsilon;
        }

        #endregion

        #region Transformation

        public GeoPosition Clone()
        {
            return new GeoPosition(Latitude, Longitude, Altitude);
        }

        public GeoPosition Move(double north, double east, double height = 0.0)
        {
            return new GeoPosition(Latitude + north, Longitude + east, Altitude + height);
        }

        public GeoPosition MoveNorth(GeoDistance distance) => Move(distance, GeoDistance.None);
        public GeoPosition MoveSouth(GeoDistance distance) => Move(-distance, GeoDistance.None);
        public GeoPosition MoveEast(GeoDistance distance) => Move(GeoDistance.None, distance);
        public GeoPosition MoveWest(GeoDistance distance) => Move(GeoDistance.None, -distance);

        public GeoPosition Move(GeoDistance north, GeoDistance east)
        {
            const double r = EarthRadiusInKilometers * 1000;

            var dn = north.Meters;
            var de = east.Meters;
            var dlat = dn / r;
            var dlon = de / (r * Math.Cos(Math.PI * Latitude / 180));

            var lat = Latitude + dlat * 180 / Math.PI;
            var lon = Longitude + dlon * 180 / Math.PI;

            return new GeoPosition(lat, lon);
        }

        public static GeoPosition FromCenterOf(IEnumerable<GeoPosition> positions)
        {
            var all = positions.Distinct().Where(p => !IsNullOrEmpty(p)).ToArray();
            var count = all.Length;

            if (count == 0)
                return null;

            if (count == 1)
                return all.First();

            double x = 0, y = 0, z = 0;

            foreach (var position in all)
            {
                var lat = ToRadians(position.Latitude);
                var lon = ToRadians(position.Longitude);

                x += Math.Cos(lat) * Math.Cos(lon);
                y += Math.Cos(lat) * Math.Sin(lon);
                z += Math.Sin(lat);
            }

            x /= count;
            y /= count;
            z /= count;

            var centerSqrt = Math.Sqrt(x * x + y * y);
            var centerLat = Math.Atan2(z, centerSqrt);
            var centerLon = Math.Atan2(y, x);

            var a = ToDegrees(centerLat);
            var b = ToDegrees(centerLon);

            return new GeoPosition(a, b);
        }

        #endregion
    }
}
