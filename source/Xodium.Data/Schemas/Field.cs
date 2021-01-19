using System;

namespace Xodium.Data.Schemas
{
    public abstract class Field<T> : IField<T>
    {
        protected Field(string name)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public string Name { get; }
        public Type FieldType => typeof(T);
        public string DataTypeName => typeof(T).Name;
        public virtual object DefaultValue => default(T);

        public abstract object Parse(string value);
        public virtual T GetValue(object value) => (T)value;
        public virtual string AsString(object value) => value.ToString();
        public virtual char AsChar(object value) => Convert.ToChar(AsByte(value));
        public virtual byte AsByte(object value) => Convert.ToByte(AsString(value));
        public virtual short AsShort(object value) => Convert.ToInt16(AsString(value));
        public virtual int AsInteger(object value) => Convert.ToInt32(AsString(value));
        public virtual long AsLong(object value) => Convert.ToInt64(AsString(value));
        public virtual float AsSingle(object value) => Convert.ToSingle(AsString(value));
        public virtual double AsDouble(object value) => Convert.ToDouble(AsString(value));
        public virtual bool AsBoolean(object value) => Convert.ToBoolean(AsString(value));
        public virtual DateTime AsDateTime(object value) => Convert.ToDateTime(AsString(value));
    }
}
