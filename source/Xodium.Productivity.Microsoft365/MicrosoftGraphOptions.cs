namespace Xodium.Productivity.Microsoft365
{
    public class MicrosoftGraphOptions
    {
        public string[] AppointmentCustomPropertyNames { get; set; }

        public static MicrosoftGraphOptions Empty { get; } = new MicrosoftGraphOptions();
    }
}
