using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Data.Schemas;
using Xodium.Data.Sequences;

namespace Xodium.Data.Transport.Json.Newtonsoft
{
    public sealed class JsonDataRecordReader : IDataRecordReader
    {
        private readonly JsonTextReader reader;
        private readonly RecordBuilder recordBuilder;

        public JsonDataRecordReader(Stream stream, ISchema schema)
        {
            reader = new JsonTextReader(new StreamReader(stream ?? throw new ArgumentNullException(nameof(stream))));
            recordBuilder = new RecordBuilder(schema ?? throw new ArgumentNullException(nameof(schema)));
        }

        public void Dispose()
        {
        }

        public async Task<IDataRecord> ReadNextRecord(CancellationToken cancellationToken)
        {
            if (SkipUntilObjectStart() && await JToken.ReadFromAsync(reader) is JObject obj)
            {
                return recordBuilder.BuildRecord(obj);
            }

            return null;
        }

        private bool SkipUntilObjectStart()
        {
            while (reader.TokenType != JsonToken.StartObject)
            {
                if (!reader.Read())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
