namespace Xodium.Data.Schemas
{
    public class BooleanField : Field<bool>
    {
        public BooleanField(string name) : base(name)
        {
        }

        public override object Parse(string value) => bool.Parse(value);
        public override bool AsBoolean(object value) => GetValue(value);
    }
}
