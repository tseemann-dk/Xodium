using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Productivity.Microsoft365.Extensions;
using Xodium.Productivity.Scheduling;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Scheduling
{
    public class CalendarManager : ICalendarManager
    {
        private readonly MSGraph.IUserRequestBuilder user;

        public CalendarManager(MSGraph.IUserRequestBuilder user)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
        }

        private Calendar Map(MSGraph.Calendar value) => new Calendar(value);
        private MSGraph.Calendar Map(ICalendar value) => value.ToCalendar();

        public async Task<ICalendar> CreateCalendar(ICalendar template, CancellationToken cancellationToken = default)
        {
            return Map(await user.Calendars.Request().AddAsync(Map(template), cancellationToken));
        }

        public async Task DeleteCalendar(ICalendar calendar, CancellationToken cancellationToken = default)
        {
            await user.Calendars[calendar.Id].Request().DeleteAsync(cancellationToken);
        }

        public async Task<ICalendar> GetCalendar(string id, CancellationToken cancellationToken = default)
        {
            return Map(await user.Calendars[id].Request().GetAsync(cancellationToken));
        }

        public async Task<IReadOnlyList<ICalendar>> GetCalendars(CancellationToken cancellationToken = default)
        {
            return (await user.Calendars.Request().GetAsync(cancellationToken)).Select(Map).ToList();
        }

        public async Task<ICalendar> GetDefaultCalendar(CancellationToken cancellationToken = default)
        {
            return Map(await user.Calendar.Request().GetAsync(cancellationToken));
        }

        public async Task<ICalendar> UpdateCalendar(ICalendar calendar, ICalendar patch)
        {
            return Map(await user.Calendars[calendar.Id].Request().UpdateAsync(Map(patch)));
        }
    }
}
