namespace Xodium.Productivity.Scheduling
{
    public class CalendarTemplate : ICalendar
    {
        public CalendarTemplate(string name)
        {
            Name = name;
        }

        public string Id => string.Empty;
        public string Name { get; set; }
    }
}
