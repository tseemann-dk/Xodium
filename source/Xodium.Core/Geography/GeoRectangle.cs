using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Geography
{
    public class GeoRectangle
    {
        public GeoRectangle(double north, double west, double south, double east)
            : this(new GeoPosition(north, west), new GeoPosition(south, east))
        {
        }

        public GeoRectangle(GeoPosition northWest, GeoPosition southEast)
        {
            NorthWest = northWest;
            SouthEast = southEast;
        }
      
        public GeoPosition NorthWest { get; set; }
        public GeoPosition SouthEast { get; set; }
        public GeoPosition NorthEast => new GeoPosition(North, East);
        public GeoPosition SouthWest => new GeoPosition(South, West);

        public double North => NorthWest.Latitude;
        public double West => NorthWest.Longitude;
        public double South => SouthEast.Latitude;
        public double East => SouthEast.Longitude;

        public double LatitudeDegrees => North - South;
        public double LongitudeDegrees => East - West;
        public double LatitudeCenter => South + LatitudeDegrees / 2;
        public double LongitudeCenter => West + LongitudeDegrees / 2;
        public GeoPosition Center => new GeoPosition(LatitudeCenter, LongitudeCenter);
        public GeoDistance Height => GeoDistance.Between(NorthWest, SouthWest);
        public GeoDistance Width => GeoDistance.Between(NorthWest, NorthEast);

        public static GeoRectangle Empty = new GeoRectangle(GeoPosition.Empty, GeoPosition.Empty);

        #region Comparison

        public override bool Equals(object obj)
        {
            if (!(obj is GeoRectangle other)) return false;
            return other.NorthWest.Equals(NorthWest) && other.SouthEast.Equals(SouthEast);
        }

        public override int GetHashCode()
        {
            var hashCode = 1124903864;
            hashCode = hashCode * -1521134295 + North.GetHashCode();
            hashCode = hashCode * -1521134295 + West.GetHashCode();
            hashCode = hashCode * -1521134295 + South.GetHashCode();
            hashCode = hashCode * -1521134295 + East.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Conversion

        public static GeoRectangle FromCenterAndRadius(GeoPosition center, GeoDistance radius)
        {
            var north = center.MoveNorth(radius).Latitude;
            var south = center.MoveNorth(-radius).Latitude;
            var east = center.MoveEast(radius).Longitude;
            var west = center.MoveEast(-radius).Longitude;

            return new GeoRectangle(north, west, south, east);
        }

        public static GeoRectangle FromCenterAndDimensions(GeoPosition center, double width, double height)
        {
            var north = center.Latitude + height / 2;
            var south = center.Latitude - height / 2;
            var east = center.Longitude + width / 2;
            var west = center.Longitude - width / 2;

            return new GeoRectangle(north, west, south, east);
        }

        public static GeoRectangle FromCenterAndDimensions(GeoPosition center, GeoDistance width, GeoDistance height)
        {
            var north = center.MoveNorth(height / 2).Latitude;
            var south = center.MoveSouth(height / 2).Latitude;
            var east = center.MoveEast(width / 2).Longitude;
            var west = center.MoveWest(width / 2).Longitude;

            return new GeoRectangle(north, west, south, east);
        }

        public static GeoRectangle FromContainedPositions(IEnumerable<GeoPosition> positions)
        {
            var all = positions?.Distinct().Where(p => !GeoPosition.IsNullOrEmpty(p)).ToArray();

            if (all == null || !all.Any())
                return Empty;

            var north = all.Max(p => p.Latitude);
            var south = all.Min(p => p.Latitude);
            var east = all.Max(p => p.Longitude);
            var west = all.Min(p => p.Longitude);

            return new GeoRectangle(north, west, south, east);
        }

        public bool Intersects(GeoRectangle target)
        {
            var lat = (Math.Abs(target.LatitudeDegrees - LatitudeDegrees) / LatitudeDegrees <= 0.2);
            var lon = (Math.Abs(target.LongitudeDegrees - LongitudeDegrees) / LongitudeDegrees <= 0.2);
            var center =(target.Center.DistanceFrom(Center).Meters <= 50);
            return lat && lon && center;
        }

        public override string ToString() => $"({NorthWest})-({SouthEast})";

        #endregion

        #region Transformation

        public GeoRectangle Expand(GeoDistance distance) => Expand(distance, distance);
        public GeoRectangle Expand(GeoDistance width, GeoDistance height) => new GeoRectangle(NorthWest.Move(height, -width), SouthEast.Move(-height, width));
        public GeoRectangle Shrink(GeoDistance distance) => Expand(-distance);
        public GeoRectangle Shrink(GeoDistance width, GeoDistance height) => Expand(-width, -height);

        #endregion
    }
}
