namespace Xodium.Productivity.Scheduling
{
    public interface IScheduleManager
    {
        ICalendarManager CalendarManager { get; }
        IAppointmentManager AppointmentManager { get; }
    }
}
