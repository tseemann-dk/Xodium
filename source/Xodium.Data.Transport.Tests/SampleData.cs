using System.IO;
using System.Text;
using Xodium.Data.Schemas;

namespace Xodium.Data.Transport.Tests
{
    public static class SampleData
    {
        public static ISchema GetSchema() => new Schema(new IField[]
        {
            new IntegerField("id"),
            new StringField("name"),
            new BooleanField("active"),
            new DoubleField("location.latitude"),
            new DoubleField("location.longitude")
        });

        public static string GetJson() => "[" +
            "{\"id\": 1, \"name\": \"Item 1\", \"active\": true, \"location\": {\"latitude\": 55.1, \"longitude\": 12.1}}," +
            "{\"id\": 2, \"name\": \"Item 2\", \"active\": false, \"location\": {\"latitude\": 55.2, \"longitude\": 12.2}}," +
            "{\"id\": 3, \"name\": \"Item 3\", \"active\": true, \"location\": {\"latitude\": 55.3, \"longitude\": 12.3}}" +
        "]";

        public static Stream GetJsonAsStream() => new MemoryStream(Encoding.UTF8.GetBytes(GetJson()));
    }
}
