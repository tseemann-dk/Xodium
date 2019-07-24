using System;
using System.Globalization;

namespace Xodium.Primitives
{
    public static class DateTimeExtensions
    {
        public static int GetWeekNumber(this DateTime time)
        {
            var culture = CultureInfo.CurrentCulture;
            var format = culture.DateTimeFormat;
            return culture.Calendar.GetWeekOfYear(time, format.CalendarWeekRule, format.FirstDayOfWeek);
        }

        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            var delta = DayOfWeek.Monday - date.DayOfWeek;
            if (delta > 0) delta -= 7;
            return date.Date.AddDays(delta);
        }

        public static bool IsWeekday(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static bool IsWeekday(this DateTimeOffset date)
        {
            return date.Date.IsWeekday();
        }

        public static DateTime GetFirstDayOfWeek(this DateTimeOffset date)
        {
            return date.DateTime.GetFirstDayOfWeek();
        }

        public static bool IsSameWeek(this DateTime date, DateTime other)
        {
            return date.GetFirstDayOfWeek().Date == other.GetFirstDayOfWeek().Date;
        }

        public static bool IsSameWeek(this DateTimeOffset date, DateTimeOffset other)
        {
            return date.DateTime.IsSameWeek(other.DateTime);
        }

        public static DateTime Round(this DateTime value, TimeSpan span)
        {
            var ticks = (value.Ticks + span.Ticks / 2 + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Floor(this DateTime value, TimeSpan span)
        {
            var ticks = value.Ticks / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTime Ceil(this DateTime value, TimeSpan span)
        {
            var ticks = (value.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }

        public static DateTimeOffset Round(this DateTimeOffset value, TimeSpan span)
        {
            return new DateTimeOffset(value.DateTime.Round(span));
        }

        public static DateTimeOffset Floor(this DateTimeOffset value, TimeSpan span)
        {
            return new DateTimeOffset(value.DateTime.Floor(span));
        }

        public static DateTimeOffset Ceil(this DateTimeOffset value, TimeSpan span)
        {
            return new DateTimeOffset(value.DateTime.Ceil(span));
        }

        public static bool IsBetween(this DateTime value, DateTime start, DateTime end)
        {
            return value >= start && value <= end;
        }
    }
}
