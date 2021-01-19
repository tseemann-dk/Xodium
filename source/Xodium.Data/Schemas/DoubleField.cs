namespace Xodium.Data.Schemas
{
    public class DoubleField : Field<double>
    {
        public DoubleField(string name) : base(name)
        {
        }

        public override object Parse(string value) => double.Parse(value);
        public override double AsDouble(object value) => GetValue(value);
        public override float AsSingle(object value) => (float)AsDouble(value);
    }
}
