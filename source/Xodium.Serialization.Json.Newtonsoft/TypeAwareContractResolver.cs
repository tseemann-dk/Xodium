using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Xodium.Serialization.Json.Newtonsoft
{
    public class TypeAwareContractResolver : DefaultContractResolver
    {
        private readonly string typeDiscriminator;

        public TypeAwareContractResolver(string typeDiscriminator)
        {
            this.typeDiscriminator = typeDiscriminator ?? throw new ArgumentNullException(nameof(typeDiscriminator));
            NamingStrategy = new CamelCaseNamingStrategy();
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization)
                .Where(ShouldIncludeProperty)
                .OrderBy(x => !IsTypeDiscriminator(x))
                .ToList();
        }

        protected bool ShouldIncludeProperty(JsonProperty property)
        {
            return IsTypeDiscriminator(property) || property.Writable;
        }

        protected bool IsTypeDiscriminator(JsonProperty property)
        {
            return string.Compare(property.PropertyName, typeDiscriminator, true, CultureInfo.InvariantCulture) == 0;
        }
    }
}
