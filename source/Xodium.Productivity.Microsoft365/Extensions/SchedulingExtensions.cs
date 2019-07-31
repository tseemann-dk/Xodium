using System;
using System.Collections.Generic;
using Microsoft.Graph;
using Xodium.Productivity.Microsoft365.Utilities;
using Xodium.Productivity.Scheduling;

namespace Xodium.Productivity.Microsoft365.Extensions
{
    public static class SchedulingExtensions
    {
        public static Calendar ToCalendar(this ICalendar calendar)
        {
            return new Calendar
            {
                Name = calendar.Name
            };
        }

        public static Event ToEvent(this IAppointment appointment, string extendedPropertiesNamespaceId)
        {
            return new Event
            {
                Subject = appointment.Subject,
                Location = appointment.Location?.ToLocation(),
                Start = appointment.StartTime?.ToDateTimeTimeZoneUtc(),
                End = appointment.EndTime?.ToDateTimeTimeZoneUtc(),
                Body = appointment.GetContentBody(),
                SingleValueExtendedProperties = ConvertCustomPropertiesToExtendedProperties(appointment.CustomProperties, extendedPropertiesNamespaceId)
            };
        }

        public static DateTimeTimeZone ToDateTimeTimeZoneUtc(this DateTimeOffset self)
        {
            return DateTimeTimeZone.FromDateTimeOffset(self.ToUniversalTime(), TimeZoneInfo.Utc.Id);
        }

        private static IEventSingleValueExtendedPropertiesCollectionPage ConvertCustomPropertiesToExtendedProperties(IReadOnlyDictionary<string, object> customProperties, string namespaceId)
        {
            var result = new EventSingleValueExtendedPropertiesCollectionPage();

            foreach (var customProperty in customProperties)
            {
                result.Add(new StringProperty(
                    namespaceId, customProperty.Key, customProperty.Value.ToString())
                    .ToSingleValueLegacyExtendedProperty());
            }

            return result;
        }

        public static SingleValueLegacyExtendedProperty ToSingleValueLegacyExtendedProperty(this ExtendedProperty self)
        {
            return new SingleValueLegacyExtendedProperty
            {
                Id = self.Id,
                Value = self.Value
            };
        }

        public static ItemBody GetContentBody(this IAppointment appointment)
        {
            if (string.IsNullOrEmpty(appointment.Content))
                return null;

            return new ItemBody
            {
                ContentType = appointment.ContentType.ToBodyType(),
                Content = appointment.Content
            };
        }

        public static BodyType ToBodyType(this Productivity.Scheduling.ContentType contentType)
        {
            switch (contentType)
            {
                case Productivity.Scheduling.ContentType.Html:
                    return BodyType.Html;
                default:
                    return BodyType.Text;
            }
        }

        public static Productivity.Scheduling.ContentType GetContentType(this Event self)
        {
            switch (self.Body?.ContentType)
            {
                case BodyType.Html:
                    return Productivity.Scheduling.ContentType.Html;
                default:
                    return Productivity.Scheduling.ContentType.Text;
            }
        }
    }
}
