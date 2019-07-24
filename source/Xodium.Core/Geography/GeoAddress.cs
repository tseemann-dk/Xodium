namespace Xodium.Geography
{
    public class GeoAddress
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryOrRegion { get; set; }

        private static string Combine(string s1, string s2, string separator)
        {
            if (s1 == null) return s2;
            return s2 == null ? s1 : s1 + separator + s2;
        }

        public string GetFullCity() => Combine(PostalCode, City, " ");
        public string GetFullCountry() => Combine(State, CountryOrRegion, ", ");
        public string GetCityAndCountry() => Combine(GetFullCity(), GetFullCountry(), ", ");
        public override string ToString() => Combine(Street, GetCityAndCountry(), ", ");
    }
}
