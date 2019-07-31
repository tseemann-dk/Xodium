using System;
using System.Collections.Generic;
using Xodium.Geography;

namespace Xodium.Productivity.Scheduling
{
    public class AppointmentTemplate : IAppointment
    {
        public AppointmentTemplate(
            string subject, 
            DateTimeOffset startTime, DateTimeOffset endTime, bool isAllDay, 
            Availability availability, Sensitivity sensitivity, Importance importance)
        {
            Subject = subject;
            StartTime = startTime;
            EndTime = endTime;
            IsAllDay = isAllDay;
            Availability = availability;
            Sensitivity = sensitivity;
            Importance = Importance;
            ContentType = ContentType.Text;
            CustomProperties = new Dictionary<string, object>();
        }

        public string Id => string.Empty;
        public string Subject { get; set; }
        public GeoLocation Location { get; set; }
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public Availability Availability { get; set; }
        public Sensitivity Sensitivity { get; set; }
        public Importance Importance { get; set; }
        public string Content { get; set; }
        public ContentType ContentType { get; set; }

        IReadOnlyDictionary<string, object> IAppointment.CustomProperties => CustomProperties;
        public Dictionary<string, object> CustomProperties { get; set; }
    }
}
