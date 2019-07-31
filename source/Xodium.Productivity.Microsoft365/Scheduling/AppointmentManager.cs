using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;
using Xodium.Productivity.Common;
using Xodium.Productivity.Microsoft365.Extensions;
using Xodium.Productivity.Microsoft365.Utilities;
using Xodium.Productivity.Scheduling;

namespace Xodium.Productivity.Microsoft365.Scheduling
{
    public class AppointmentManager : IAppointmentManager
    {
        private const string ExtendedPropertiesDefaultNamespaceId = "{24055152-7fb4-4e4b-865b-c1ac71aab868}";

        private readonly IUserRequestBuilder user;
        private readonly IEnumerable<string> extendedPropertyNames;
        private readonly string extendedPropertiesNamespaceId;

        public AppointmentManager(IUserRequestBuilder user, IEnumerable<string> extendedPropertyNames, string extendedPropertiesNamespaceId = null)
        {
            this.user = user ?? throw new ArgumentNullException(nameof(user));
            this.extendedPropertyNames = extendedPropertyNames;
            this.extendedPropertiesNamespaceId = extendedPropertiesNamespaceId ?? ExtendedPropertiesDefaultNamespaceId;
        }

        public int QueryPageSize { get; set; } = 10;

        #region Public Methods

        public async Task<IAppointment> CreateAppointment(ICalendar calendar, IAppointment template, CancellationToken cancellationToken)
        {
            var newEvent = await CreateEvent(calendar.Id, Map(template), cancellationToken);

            if (template.CustomProperties != null)
            {
                await UpdateEventExtendedProperties(
                    newEvent.Id,
                    template.CustomProperties,
                    cancellationToken);
            }

            return Map(newEvent);
        }

        public Task DeleteAppointment(IAppointment appointment, CancellationToken cancellationToken)
        {
            return DeleteEvent(appointment.Id, cancellationToken);
        }

        public async Task<IAppointment> GetAppointment(string id, CancellationToken cancellationToken)
        {
            return Map(await GetEvent(id, cancellationToken));
        }

        public Task<IPage<IAppointment>> GetAppointments(
            ICalendar calendar,
            DateTimeOffset from,
            DateTimeOffset to,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return GetAppointmentsFromCalendarView(calendar, from, to, null, cancellationToken);
        }

        public Task<IPage<IAppointment>> GetAppointmentsByCustomProperty(string propertyName, object value, CancellationToken cancellationToken)
        {
            return GetAppointments(BuildExtendedPropertyFilterOption(propertyName, value), cancellationToken);
        }

        public Task<IPage<IAppointment>> GetAppointmentsByCustomProperty(ICalendar calendar, string propertyName, object value, CancellationToken cancellationToken)
        {
            return GetAppointmentsFromCalendar(calendar, BuildExtendedPropertyFilterOption(propertyName, value), cancellationToken);
        }

        public Task<IPage<IAppointment>> GetAppointmentsByFilter(string filter, CancellationToken cancellationToken)
        {
            return GetAppointments(BuildFilterOption(filter), cancellationToken);
        }

        public Task<IPage<IAppointment>> GetAppointmentsByFilter(ICalendar calendar, string filter, CancellationToken cancellationToken)
        {
            return GetAppointmentsFromCalendar(calendar, BuildFilterOption(filter), cancellationToken);
        }

        public async Task<IAppointment> UpdateAppointment(IAppointment appointment, IAppointment template, CancellationToken cancellationToken)
        {
            return Map(await UpdateEvent(appointment.Id, Map(template), cancellationToken));
        }

        #endregion

        #region Mapping

        private Appointment Map(Event value) => new Appointment(value);
        private Event Map(IAppointment value) => value.ToEvent(extendedPropertiesNamespaceId);

        #endregion

        #region Query Option Helpers

        private IEnumerable<QueryOption> AddDefaultQueryOptions(IEnumerable<QueryOption> options)
            => options
            .AddExpanders(extendedPropertyNames, extendedPropertiesNamespaceId)
            .AddPageSize(QueryPageSize);

        private IEnumerable<QueryOption> BuildFilterOption(string filter)
            => QueryOptionExtensions.EmptyOptions.AddFilter(filter);

        private IEnumerable<QueryOption> BuildExtendedPropertyFilterOption(string propertyName, object value)
            => QueryOptionExtensions.EmptyOptions.AddExtendedPropertyFilter(propertyName, value.ToString(), extendedPropertiesNamespaceId);

        private IEnumerable<QueryOption> BuildQueryOptions(IEnumerable<KeyValuePair<string, string>> arguments = null)
            => AddDefaultQueryOptions((arguments ?? new Dictionary<string, string>()).ToOptions());

        #endregion

        #region Internal Query Methods

        private Task<IPage<IAppointment>> GetAppointments(IEnumerable<QueryOption> options, CancellationToken cancellationToken)
        {
            return ToAppointments(user
                .Events
                .Request(AddDefaultQueryOptions(options))
                .GetAsync(cancellationToken),
                cancellationToken);
        }

        private Task<IPage<IAppointment>> GetAppointmentsFromCalendar(ICalendar calendar, IEnumerable<QueryOption> options, CancellationToken cancellationToken)
        {
            return ToAppointments(user
                .Calendars[calendar.Id]
                .Events
                .Request(AddDefaultQueryOptions(options))
                .GetAsync(cancellationToken),
                cancellationToken);
        }

        private Task<IPage<IAppointment>> GetAppointmentsFromCalendarView(
            ICalendar calendar,
            DateTimeOffset from,
            DateTimeOffset to,
            IEnumerable<QueryOption> options, 
            CancellationToken cancellationToken)
        {
            var baseOptions = new[]
            {
                new QueryOption("startDateTime", ToUtcString(from)),
                new QueryOption("endDateTime", ToUtcString(to))
            };

            var allOptions = options == null
                ? baseOptions
                : baseOptions.Concat(options);

            return ToAppointments(user
                .Calendars[calendar.Id]
                .CalendarView
                .Request(AddDefaultQueryOptions(allOptions))
                .GetAsync(cancellationToken),
                cancellationToken);
        }

        private string ToUtcString(DateTimeOffset value) => value.ToUniversalTime().DateTime.ToString("o");

        private Task<IPage<IAppointment>> ToAppointments(Task<IUserEventsCollectionPage> events, CancellationToken cancellationToken)
        {
            return EventsToAppointments(events, (page, ct) => page?.NextPageRequest?.GetAsync(ct), cancellationToken);
        }

        private Task<IPage<IAppointment>> ToAppointments(Task<ICalendarEventsCollectionPage> events, CancellationToken cancellationToken)
        {
            return EventsToAppointments(events, (page, ct) => page?.NextPageRequest?.GetAsync(ct), cancellationToken);
        }

        private Task<IPage<IAppointment>> ToAppointments(Task<ICalendarCalendarViewCollectionPage> events, CancellationToken cancellationToken)
        {
            return EventsToAppointments(events, (page, ct) => page?.NextPageRequest?.GetAsync(ct), cancellationToken);
        }

        private async Task<IPage<IAppointment>> EventsToAppointments<T>(Task<T> retriever, Func<T, CancellationToken, Task<T>> nextPage, CancellationToken cancellationToken)
            where T : ICollectionPage<Event>
        {
            if (retriever == null) return null;
            var events = await retriever;
            var appointments = events.Select(Map);
            return new Page<IAppointment>(appointments, EventsToAppointments(nextPage(events, cancellationToken), nextPage, cancellationToken));
        }

        #endregion

        #region Internal Event Operations

        private Task<Event> CreateEvent(string calendarId, Event template, CancellationToken cancellationToken)
        {
            return user.Calendars[calendarId].Events.Request().AddAsync(template, cancellationToken);
        }

        private Task DeleteEvent(string id, CancellationToken cancellationToken)
        {
            return user.Events[id].Request().DeleteAsync(cancellationToken);
        }

        private Task<Event> GetEvent(string id, CancellationToken cancellationToken)
        {
            return user.Events[id].Request(BuildQueryOptions()).GetAsync(cancellationToken);
        }

        private Task<Event> UpdateEvent(string id, Event patch, CancellationToken cancellationToken)
        {
            return user.Events[id].Request().UpdateAsync(patch, cancellationToken);
        }

        private Task<Event> UpdateEventExtendedProperties(string id, IEnumerable<KeyValuePair<string, object>> properties, CancellationToken cancellationToken)
        {
            var extendedProperties = properties.Select(x => new StringProperty(extendedPropertiesNamespaceId, x.Key, x.Value.ToString()));
            var singleValueProperties = new EventSingleValueExtendedPropertiesCollectionPage();

            foreach (var property in extendedProperties)
            {
                singleValueProperties.Add(property.ToSingleValueLegacyExtendedProperty());
            }
            
            var patch = new Event
            {
                Id = id,
                SingleValueExtendedProperties = singleValueProperties
            };

            return user.Events[id].Request().UpdateAsync(patch, cancellationToken);
        }

        #endregion
    }
}
