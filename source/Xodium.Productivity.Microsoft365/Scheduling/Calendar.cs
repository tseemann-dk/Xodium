using Xodium.Productivity.Scheduling;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Scheduling
{
    public class Calendar : ICalendar
    {
        private readonly MSGraph.Calendar calendar;

        public Calendar(MSGraph.Calendar calendar)
        {
            this.calendar = calendar;
        }

        public string Id => calendar.Id;
        public string Name => calendar.Name;
    }
}
