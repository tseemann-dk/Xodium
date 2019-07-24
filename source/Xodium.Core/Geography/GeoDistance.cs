namespace Xodium.Geography
{
    public struct GeoDistance
    {
        private const double MetersPerMile = 1609.344;
        private const double MetersPerKilometer = 1000;

        public GeoDistance(double meters)
        {
            Meters = meters;
        }

        public static readonly GeoDistance None = FromMeters(0);

        public double Meters { get; }
        public double Kilometers => Meters / MetersPerKilometer;
        public double Miles => Meters / MetersPerMile;

        public static GeoDistance FromMeters(double meters) => new GeoDistance(meters);
        public static GeoDistance FromKilometers(double kilometers) => new GeoDistance(kilometers * MetersPerKilometer);
        public static GeoDistance FromMiles(double miles) => new GeoDistance(miles * MetersPerMile);

        public static GeoDistance Between(GeoPosition a, GeoPosition b) => a.DistanceFrom(b);

        public bool Equals(GeoDistance other) => Meters.Equals(other.Meters);
        public override bool Equals(object obj) => obj is GeoDistance other && Equals(other);
        public override int GetHashCode() => Meters.GetHashCode();

        public static bool operator ==(GeoDistance left, GeoDistance right) => left.Equals(right);
        public static bool operator !=(GeoDistance left, GeoDistance right) => !left.Equals(right);
        public static GeoDistance operator +(GeoDistance left, GeoDistance right) => new GeoDistance(left.Meters + right.Meters);
        public static GeoDistance operator +(GeoDistance distance, double addition) => new GeoDistance(distance.Meters * addition);
        public static GeoDistance operator *(GeoDistance distance, double multiplier) => new GeoDistance(distance.Meters * multiplier);
        public static GeoDistance operator /(GeoDistance distance, double divider) => new GeoDistance(distance.Meters / divider);
        public static GeoDistance operator -(GeoDistance distance) => new GeoDistance(-distance.Meters);
    }
}
