namespace Xodium.Data.Schemas
{
    public class IntegerField : Field<int>
    {
        public IntegerField(string name) : base(name)
        {
        }

        public override object Parse(string value) => long.Parse(value);
        public override long AsLong(object value) => GetValue(value);
        public override int AsInteger(object value) => (int)AsLong(value);
        public override byte AsByte(object value) => (byte)AsInteger(value);
    }
}
