namespace Xodium.Geography
{
    public class GeoLocation
    {
        public string Name { get; set; }
        public GeoAddress Address { get; set; }
        public GeoPosition Position { get; set; }

        public override string ToString()
        {
            return Name 
                ?? Address?.ToString() 
                ?? Position?.ToString();
        }
    }
}
