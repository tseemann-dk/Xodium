using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Productivity.Scheduling
{
    public interface ICalendarManager
    {
        Task<ICalendar> CreateCalendar(ICalendar template, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteCalendar(ICalendar calendar, CancellationToken cancellationToken = default(CancellationToken));
        Task<ICalendar> GetCalendar(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IReadOnlyList<ICalendar>> GetCalendars(CancellationToken cancellationToken = default(CancellationToken));
        Task<ICalendar> GetDefaultCalendar(CancellationToken cancellationToken = default(CancellationToken));
        Task<ICalendar> UpdateCalendar(ICalendar calendar, ICalendar template);
    }
}
