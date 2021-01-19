using System.IO;
using Xodium.Data.Schemas;
using Xodium.Data.Sequences;
using Xodium.Data.Transport.Json.Newtonsoft;

namespace Xodium.Data.Transport.Json.Newtonsoft
{
    public class JsonDataReader : AsyncDataReader
    {
        public JsonDataReader(Stream stream, ISchema schema) 
            : base(() => new JsonDataRecordReader(stream, schema))
        {
        }
    }
}
