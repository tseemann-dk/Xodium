using System;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Microsoft365.Utilities;

namespace Xodium.Productivity.Microsoft365.Extensions
{
    public static class QueryOptionExtensions
    {
        const string ExtendedPropertiesFieldName = "singleValueExtendedProperties";
        const string ExtendedPropertiesMnemonic = "ep";

        private static string GetExtendedPropertyFilterExpression(ExtendedProperty property)
            => GetExtendedPropertyFilterExpression(property, ExtendedPropertiesFieldName, ExtendedPropertiesMnemonic);
        private static string GetExtendedPropertyFilterExpression(ExtendedProperty property, string fieldName, string mnemonic)
            => $"{fieldName}/any({mnemonic}: {mnemonic}/id eq '{property.Id}' and {mnemonic}/value eq '{property.Value}')";

        private static string GetExtendedPropertyExpandExpression(ExtendedProperty property)
            => GetExtendedPropertyExpandExpression(property, ExtendedPropertiesFieldName);
        private static string GetExtendedPropertyExpandExpression(ExtendedProperty property, string fieldName)
            => $"{fieldName}($filter=id eq '{property.Id}')";

        private static QueryOption BuildQueryOption(string name, string value)
            => new QueryOption(name, Uri.EscapeDataString(value));

        private static QueryOption BuildExpandOption(string propertyName, string namespaceId)
            => BuildQueryOption("$expand", GetExtendedPropertyExpandExpression(new StringProperty(namespaceId, propertyName, null)));

        private static QueryOption BuildExtendedPropertyFilterOption(string propertyName, string value, string namespaceId)
            => BuildQueryOption("$filter", GetExtendedPropertyFilterExpression(new StringProperty(namespaceId, propertyName, value)));

        public static IEnumerable<QueryOption> EmptyOptions => Enumerable.Empty<QueryOption>();

        public static IEnumerable<QueryOption> ToOptions(this IEnumerable<KeyValuePair<string, string>> arguments)
            => arguments.Select(x => BuildQueryOption(x.Key, x.Value));

        public static IEnumerable<QueryOption> AddOption(this IEnumerable<QueryOption> options, QueryOption option)
            => options.Concat(new[] { option });

        public static IEnumerable<QueryOption> AddExpanders(this IEnumerable<QueryOption> options, IEnumerable<string> propertyNames, string namespaceId)
            => propertyNames == null ? options : options.Concat(propertyNames.Select(x => BuildExpandOption(x, namespaceId)));

        public static IEnumerable<QueryOption> AddExtendedPropertyFilter(this IEnumerable<QueryOption> options, string propertyName, string value, string namespaceId)
            => options.AddOption(BuildExtendedPropertyFilterOption(propertyName, value, namespaceId));

        public static IEnumerable<QueryOption> AddFilter(this IEnumerable<QueryOption> options, string filter)
            => options.AddOption(BuildQueryOption("$filter", filter));

        public static IEnumerable<QueryOption> AddPageSize(this IEnumerable<QueryOption> options, int pageSize)
            => options.AddOption(BuildQueryOption("$top", pageSize.ToString()));
    }
}
