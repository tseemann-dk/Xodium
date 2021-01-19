namespace Xodium.Data.Schemas
{
    public class StringField : Field<string>
    {
        public StringField(string name) : base(name)
        {
        }

        public override object Parse(string value) => value;
        public override string AsString(object value) => GetValue(value);
    }
}
