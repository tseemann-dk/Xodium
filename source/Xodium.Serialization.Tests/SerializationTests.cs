using FluentAssertions;
using Nsj = Newtonsoft.Json;
using Stj = System.Text.Json;

namespace Xodium.Serialization.Tests
{
    public class SerializationTests
    {
        [Theory]
        [MemberData(nameof(Serializers))]
        public async Task CanSerializeAndDeserializePlainObject(ISerializer serializer)
        {
            var obj1 = CreatePerson();
            var str = await serializer.SerializeAsync(obj1);
            var obj2 = await serializer.DeserializeAsync<Person>(str);

            obj2.Should().BeEquivalentTo(obj1);
        }

        [Theory]
        [MemberData(nameof(Serializers))]
        public async Task CanSerializeAndDeserializeUntypedData(ISerializer serializer)
        {
            var obj1 = CreateUntypedData();
            var str = await serializer.SerializeAsync(obj1);
            var obj2 = await serializer.DeserializeAsync<UntypedData>(str);

            obj2.Should().BeEquivalentTo(obj1);
        }

        public static IEnumerable<object[]> Serializers => new[]
        {
            new object[] { CreateCoreFXJsonSerializer() },
            new object[] { CreateNewtonsoftJsonSerializer() }
        };

        private static Json.Newtonsoft.NewtonsoftJsonSerializer CreateNewtonsoftJsonSerializer()
        {
            var settings = new Nsj.JsonSerializerSettings
            {
            };

            return new Json.Newtonsoft.NewtonsoftJsonSerializer();
        }

        private static Json.CoreFX.NativeJsonSerializer CreateCoreFXJsonSerializer()
        {
            var options = new Stj.JsonSerializerOptions
            {
                Converters =
                {
                    new Json.CoreFX.ObjectToInferredTypesConverter()
                }
            };

            return new Json.CoreFX.NativeJsonSerializer(options);
        }

        private static Person CreatePerson() => new()
        {
            Id = 1,
            Name = "John Doe"
        };

        private static UntypedData CreateUntypedData() => new()
        {
            Value1 = 123,
            Value2 = "Sample Text"
        };

        private class Person
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        private class UntypedData
        {
            public object? Value1 { get; set; }
            public object? Value2 { get; set; }
        }
    }
}