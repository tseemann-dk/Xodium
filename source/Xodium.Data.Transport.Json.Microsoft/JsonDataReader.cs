using System.IO;
using Xodium.Data.Schemas;
using Xodium.Data.Sequences;

namespace Xodium.Data.Transport.Json.Microsoft
{
    public class JsonDataReader : AsyncDataReader
    {
        public JsonDataReader(Stream stream, ISchema schema) 
            : base(() => new JsonDataRecordReader(stream, schema))
        {
        }
    }
}
