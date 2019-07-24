using System;

namespace Xodium.Geography
{
    public class TrackedPosition
    {
        public GeoPosition Position { get; set; }
        public double? Heading { get; set; }
        public double? Speed { get; set; }
        public DateTimeOffset Time { get; set; }

        public static TrackedPosition Empty = new TrackedPosition { Position = GeoPosition.Empty };

        public override string ToString()
        {
            return $"({Position}) {Heading ?? 0}° {Speed ?? 0} m/s";
        }
    }
}
