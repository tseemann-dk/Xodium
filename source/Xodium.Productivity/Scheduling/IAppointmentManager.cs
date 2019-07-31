using System;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Productivity.Common;

namespace Xodium.Productivity.Scheduling
{
    public interface IAppointmentManager
    {
        Task<IAppointment> CreateAppointment(ICalendar calendar, IAppointment template, CancellationToken cancellationToken = default(CancellationToken));
        Task DeleteAppointment(IAppointment appointment, CancellationToken cancellationToken = default(CancellationToken));
        Task<IAppointment> GetAppointment(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage<IAppointment>> GetAppointments(ICalendar calendar, DateTimeOffset from, DateTimeOffset to, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage<IAppointment>> GetAppointmentsByCustomProperty(string propertyName, object value, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage<IAppointment>> GetAppointmentsByCustomProperty(ICalendar calendar, string propertyName, object value, CancellationToken cancellationToken = default(CancellationToken));
        Task<IPage<IAppointment>> GetAppointmentsByFilter(string filter, CancellationToken cancellationToken);
        Task<IPage<IAppointment>> GetAppointmentsByFilter(ICalendar calendar, string filter, CancellationToken cancellationToken);
        Task<IAppointment> UpdateAppointment(IAppointment appointment, IAppointment template, CancellationToken cancellationToken = default(CancellationToken));
    }
}
