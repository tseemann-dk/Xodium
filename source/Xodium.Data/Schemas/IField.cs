using System;

namespace Xodium.Data.Schemas
{
    public interface IField
    {
        string Name { get; }
        Type FieldType { get; }
        string DataTypeName { get; }
        object DefaultValue { get; }
        object Parse(string value);

        string AsString(object value);
        char AsChar(object value);
        byte AsByte(object value);
        short AsShort(object value);
        int AsInteger(object value);
        long AsLong(object value);
        float AsSingle(object value);
        double AsDouble(object value);
        bool AsBoolean(object value);
        DateTime AsDateTime(object value);
    }

    public interface IField<T> : IField
    {
        T GetValue(object value);
    }
}
