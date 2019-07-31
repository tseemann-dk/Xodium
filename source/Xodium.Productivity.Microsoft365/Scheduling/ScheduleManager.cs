using Microsoft.Graph;
using System.Collections.Generic;
using Xodium.Productivity.Microsoft365.Scheduling;
using Xodium.Productivity.Scheduling;

namespace Xodium.Productivity.Outlook.Scheduling
{
    public class ScheduleManager : IScheduleManager
    {
        public ScheduleManager(IUserRequestBuilder user, IEnumerable<string> appointmentCustomPropertyNames)
        {
            AppointmentManager = new AppointmentManager(user, appointmentCustomPropertyNames);
            CalendarManager = new CalendarManager(user);
        }

        public IAppointmentManager AppointmentManager { get; }
        public ICalendarManager CalendarManager { get; }
    }
}
