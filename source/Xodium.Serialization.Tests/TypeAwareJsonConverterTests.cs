using System.Text.Json;
using FluentAssertions;
using Xodium.Serialization;
using Xodium.Serialization.Json.CoreFX;

namespace Xodium.Serialization.Tests
{
    public class TypeAwareJsonConverterTests
    {
        [Fact]
        public void Read_binds_the_discriminator_value_onto_a_matching_writable_property()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new TypeAwareJsonConverter<GenericThing>(new AnyNameResolver()) }
            };

            var result = JsonSerializer.Deserialize<GenericThing>("""{"type":"Cat","name":"Whiskers"}""", options);

            result.Should().NotBeNull();
            result!.Type.Should().Be("Cat", "the discriminator must round-trip onto a matching writable property " +
                "even when ResolveType maps multiple discriminator values to the same shared CLR type " +
                "(e.g. a registry-only kind with no dedicated facade type)");
            result.Name.Should().Be("Whiskers");
        }

        [Fact]
        public void Read_still_works_when_the_resolved_type_has_no_property_matching_the_discriminator()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new TypeAwareJsonConverter<UndiscriminatedThing>(new AnyNameResolver<UndiscriminatedThing>()) }
            };

            var result = JsonSerializer.Deserialize<UndiscriminatedThing>("""{"type":"Cat","name":"Whiskers"}""", options);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Whiskers");
        }

        private class GenericThing
        {
            public string? Type { get; set; }
            public string? Name { get; set; }
        }

        private class UndiscriminatedThing
        {
            public string? Name { get; set; }
        }

        private class AnyNameResolver : AnyNameResolver<GenericThing>
        {
        }

        private class AnyNameResolver<T> : ITypeResolver
        {
            public Type ResolveType(string assemblyName, string typeName) => typeof(T);

            public bool UnresolveType(Type type, out string assemblyName, out string typeName)
            {
                assemblyName = null!;
                typeName = null!;
                return false;
            }
        }
    }
}
