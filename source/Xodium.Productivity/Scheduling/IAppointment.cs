using System;
using System.Collections.Generic;
using Xodium.Geography;

namespace Xodium.Productivity.Scheduling
{
    public interface IAppointment
    {
        string Id { get; }
        string Subject { get; }
        DateTimeOffset? StartTime { get; }
        DateTimeOffset? EndTime { get; }
        bool IsAllDay { get; }
        Availability Availability { get; }
        Sensitivity Sensitivity { get; }
        Importance Importance { get; }
        GeoLocation Location { get; }
        string Content { get; }
        ContentType ContentType { get; }
        IReadOnlyDictionary<string, object> CustomProperties { get; }
    }
}
