using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Xodium.Data.Schemas;
using Xodium.Data.Sequences;
using Xunit;
using Native = Xodium.Data.Transport.Json.Microsoft;
using Newton = Xodium.Data.Transport.Json.Newtonsoft;

namespace Xodium.Data.Transport.Tests
{
    public class JsonDataReaderTest
    {
        public JsonDataReaderTest()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        /*
        private static async Task DownloadAndProcess(string url, ISchema schema, long count, Func<IDataRecord, Task> onProcess)
        {
            var client = new HttpClient();
            var response = (await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead)).EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new Native.JsonDataReader(stream, schema);

            await foreach (var record in reader)
            {
                if (count-- == 0)
                    break;

                await onProcess(record);
            }
        }
        */

        [Fact]
        public Task CanReadJsonAsDataRecordsUsingNativeReader()
        {
            return CanReadJsonAsDataRecords((stream, schema) => new Native.JsonDataReader(stream, schema));
        }

        [Fact]
        public Task CanReadJsonAsDataRecordsUsingNewtonsoftReader()
        {
            return CanReadJsonAsDataRecords((stream, schema) => new Newton.JsonDataReader(stream, schema));
        }

        private static async Task CanReadJsonAsDataRecords(Func<Stream, ISchema, AsyncDataReader> getReader)
        {
            if (getReader is null)
            {
                throw new ArgumentNullException(nameof(getReader));
            }

            var schema = SampleData.GetSchema();
            var output = new List<string>();

            using var stream = SampleData.GetJsonAsStream();
            using var reader = getReader(stream, schema);

            await foreach (var record in reader)
            {
                var values = new List<string>();

                for (var i = 0; i < record.FieldCount; i++)
                {
                    var name = record.GetName(i);
                    var value = record.GetValue(i);

                    values.Add($"{name}: {value}");
                }

                output.Add(string.Join(", ", values));
            }

            Assert.Equal(3, output.Count);
            Assert.Equal("id: 1, name: Item 1, active: True, location.latitude: 55.1, location.longitude: 12.1", output[0]);
            Assert.Equal("id: 2, name: Item 2, active: False, location.latitude: 55.2, location.longitude: 12.2", output[1]);
            Assert.Equal("id: 3, name: Item 3, active: True, location.latitude: 55.3, location.longitude: 12.3", output[2]);
        }
    }
}
