using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Xodium.Data.Schemas
{
    public class DataRecord : IDataRecord
    {
        private readonly ISchema schema;
        private readonly List<object> values;

        public DataRecord(ISchema schema, IEnumerable<object> values)
        {
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
            this.values = values?.ToList() ?? new List<object>();
        }

        public object this[int i] => GetValue(i);
        public object this[string name] => GetValue(GetOrdinal(name));
        public int FieldCount => schema.Fields.Count;
        public bool GetBoolean(int i) => GetField(i).AsBoolean(GetValue(i));
        public byte GetByte(int i) => GetField(i).AsByte(GetValue(i));
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public char GetChar(int i) => GetField(i).AsChar(GetValue(i));
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();
        public IDataReader GetData(int i) => throw new NotImplementedException();
        public string GetDataTypeName(int i) => schema.Fields[i].DataTypeName;
        public DateTime GetDateTime(int i) => GetField(i).AsDateTime(GetValue(i));
        public decimal GetDecimal(int i) => Convert.ToDecimal(GetString(i));
        public double GetDouble(int i) => GetField(i).AsDouble(GetValue(i));
        public Type GetFieldType(int i) => GetField(i).FieldType;
        public float GetFloat(int i) => GetField(i).AsSingle(GetValue(i));
        public Guid GetGuid(int i) => Guid.Parse(GetString(i));
        public short GetInt16(int i) => GetField(i).AsShort(GetValue(i));
        public int GetInt32(int i) => GetField(i).AsInteger(GetValue(i));
        public long GetInt64(int i) => GetField(i).AsLong(GetValue(i));
        public string GetName(int i) => schema.Fields[i].Name;
        public int GetOrdinal(string name) => schema.IndexOfField(name);
        public string GetString(int i) => GetField(i).AsString(GetValue(i));
        public object GetValue(int i) => values[i];
        public int GetValues(object[] values) => throw new NotImplementedException();
        public bool IsDBNull(int i) => GetValue(i) == null;

        private IField GetField(int i) => schema.Fields[i];
    }
}
