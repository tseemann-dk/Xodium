using System;

namespace Xodium.Data.Schemas
{
    public class DateTimeField : Field<DateTime>
    {
        public DateTimeField(string name) : base(name)
        {
        }

        public override object Parse(string value) => DateTime.Parse(value);
        public override DateTime AsDateTime(object value) => GetValue(value);
    }
}
