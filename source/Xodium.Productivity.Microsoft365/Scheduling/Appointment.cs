using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Graph.Extensions;
using Xodium.Geography;
using Xodium.Productivity.Microsoft365.Extensions;
using Xodium.Productivity.Microsoft365.Utilities;
using Xodium.Productivity.Scheduling;
using MSGraph = Microsoft.Graph;

namespace Xodium.Productivity.Microsoft365.Scheduling
{
    public class Appointment : IAppointment
    {
        private readonly MSGraph.Event instance;

        public Appointment(MSGraph.Event instance)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));

            Location = instance.Location?.ToGeoLocation();
            IsAllDay = instance.IsAllDay ?? false;
            StartTime = instance.Start.ToDateTimeOffset();
            EndTime = instance.End.ToDateTimeOffset();
            Availability = instance.ShowAs?.ToAvailability() ?? Availability.Undefined;
            Sensitivity = instance.Sensitivity?.ToSensitivity() ?? Sensitivity.Private;
            Importance = instance.Importance?.ToImportance() ?? Importance.Normal;
            ContentType = instance.GetContentType();
            Content = instance.Body?.Content;
            CustomProperties = ConvertExtendedPropertiesToCustomProperties(instance.SingleValueExtendedProperties);
        }

        private IReadOnlyDictionary<string, object> ConvertExtendedPropertiesToCustomProperties(MSGraph.IEventSingleValueExtendedPropertiesCollectionPage source)
        {
            return source?
                .Select(x => new ExtendedProperty(x.Id, x.Value))
                .ToDictionary(x => x.PropertyName, x => (object)x.Value);
        }

        public string Id => instance.Id;
        public string Subject => instance.Subject;
        public DateTimeOffset? StartTime { get; }
        public DateTimeOffset? EndTime { get; }
        public bool IsAllDay { get; }
        public Availability Availability { get; }
        public Sensitivity Sensitivity { get; }
        public Importance Importance { get; }
        public GeoLocation Location { get; }
        public string Content { get; }
        public Productivity.Scheduling.ContentType ContentType { get; }
        public IReadOnlyDictionary<string, object> CustomProperties { get; }
    }
}
