using System;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Data.Schemas;
using Xodium.Data.Sequences;

namespace Xodium.Data.Transport.Json.Microsoft
{
    public sealed class JsonDataRecordReader : IDataRecordReader
    {
        private readonly JsonObjectStream stream;
        private readonly RecordBuilder recordBuilder;

        public JsonDataRecordReader(Stream stream, ISchema schema)
        {
            this.stream = new JsonObjectStream(stream ?? throw new ArgumentNullException(nameof(stream)));
            recordBuilder = new RecordBuilder(schema ?? throw new ArgumentNullException(nameof(schema)));
        }

        public void Dispose()
        {
        }

        public async Task<IDataRecord> ReadNextRecord(CancellationToken cancellationToken)
        {
            if (stream.SkipUntilObjectStart() && await JsonDocument.ParseAsync(stream, default, cancellationToken) is JsonDocument doc)
            {
                return recordBuilder.BuildRecord(doc.RootElement);
            }

            return null;
        }
    }
}
