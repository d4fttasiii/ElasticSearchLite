using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Text;
using static ElasticSearchLite.NetCore.Queries.Search;
using static ElasticSearchLite.NetCore.Queries.Delete;

namespace ElasticSearchLite.NetCore.Queries.Generator
{
    public class StatementFactory : IStatementFactory
    {
        private static List<string> ExcludedProperties { get; } = new List<string> { "Index", "Id", "Type", "Score" };
        private IEnumerable<PropertyInfo> UpdatetableProperties(IElasticPoco poco) => poco.GetType().GetProperties().Where(p => !ExcludedProperties.Contains(p.Name));

        public string Generate(IQuery query)
        {
            switch (query)
            {
                case SearchQuery searchQuery:
                    return GenerateSearchQuery(searchQuery);
                case DeleteQuery deleteQuery:
                    return GenerateDeleteQuery(deleteQuery);
                case Index indexQuery:
                    return GenerateInsertQuery(indexQuery);
                case Update updateQuery:
                    return GenerateUpdateQuery(updateQuery);
                case Bulk bulkQuery:
                    return GenerateBulkQuery(bulkQuery);
                default:
                    throw new Exception("Unknown query type");
            }
        }

        private string GenerateSearchQuery(SearchQuery searchQuery)
        {
            var statementParts = new List<string>
            {
                GenerateQuery(searchQuery),
                GenerateSize(searchQuery.Size),
                GenerateFrom(searchQuery.From)
            };

            return $"{{ {string.Join(",", statementParts)} }}";
        }

        private string GenerateDeleteQuery(DeleteQuery deleteQuery)
        {
            return $"{{ {GenerateQuery(deleteQuery)} }}";
        }

        private string GenerateInsertQuery(Index indexQuery)
        {
            var properties = UpdatetableProperties(indexQuery.Poco);

            return $"{{ {GenerateFieldMapping(properties, indexQuery.Poco)} }}";
        }

        private string GenerateUpdateQuery(Update updateQuery)
        {
            return GenerateDocument(updateQuery.Poco);
        }

        private string GenerateBulkQuery(Bulk bulkQuery)
        {
            var statement = new StringBuilder();

            foreach ((ElasticMethods method, var poco) in bulkQuery.PocosAndMethods)
            {
                statement.AppendLine($@"{{ ""{method.Name}"": {{ ""{ElasticFields.Id.Name}"": ""{poco.Id}"", ""{ElasticFields.Index.Name}"": ""{poco.Index}"", ""{ElasticFields.Type.Name}"": ""{poco.Type}"" }} }}");

                switch (method.Name)
                {
                    case "index":
                        statement.AppendLine($"{{ {GenerateFieldMapping(UpdatetableProperties(poco), poco)} }}");
                        break;
                    case "update":
                        statement.AppendLine(GenerateDocument(poco));
                        break;
                }
            }

            return statement.ToString();
        }

        private string GenerateDocument(IElasticPoco poco)
        {
            var properties = UpdatetableProperties(poco);

            return $@"{{ ""doc"": {{ {GenerateFieldMapping(properties, poco)} }} }}";
        }

        private string GenerateFieldMapping(IEnumerable<PropertyInfo> properties, IElasticPoco poco)
        {
            var propertiesAsJson = properties
                .Where(p => p.GetValue(poco) != null)
                .Select(p => $@"""{p.Name}"": {EscapeValue(p.GetValue(poco))}");

            return string.Join(",", propertiesAsJson);
        }

        private string GenerateQuery(AbstractQuery query)
        {
            if (query.MatchCondition != null)
            {
                return $@"""query"": {{ {GenerateMatch(query.MatchCondition)} }}";
            }

            if (query.TermCondition != null)
            {
                return $@"""query"": {{ {GenerateTerm(query.TermCondition)} }}";
            }

            if (query.RangeCondition != null)
            {
                return $@"""query"": {{ {GenerateRange(query.RangeCondition)} }}";
            }

            return string.Empty;
        }

        private string EscapeValue(object value)
        {
            if (value.GetType().IsArray)
            {
                var values = (value as object[]).Select(v => EscapeValue(v));
                var joinedValues = string.Join(",", values);

                return $"[{joinedValues}]";
            }

            switch (value)
            {
                case double number:
                    return number.ToString(CultureInfo.InvariantCulture);
                case float number:
                    return number.ToString(CultureInfo.InvariantCulture);
                case int number:
                    return number.ToString();
                case long number:
                    return number.ToString();
                case byte b:
                    return b.ToString();
                case string text:
                    return $@"""{text}""";
                case DateTime date:
                    return $@"""{date.ToString("yyyy-MM-dd HH:mm:ss")}""";
                case TimeSpan time:
                    return time.ToString("HH:mm:ss");
                case bool logical:
                    return logical.ToString().ToLower();
                default:
                    throw new Exception("Unknown type as POCO property");
            }
        }

        private string GenerateMatch(ElasticTermCodition condition) => $@"""match"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        private string GenerateTerm(ElasticTermCodition condition) => $@"""term"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        private string GenerateRange(ElasticRangeCondition condition) => $@"""range"": {{""{condition.Field.Name}"": {{""{condition.Operation.Name}"" : ""{condition.Value}"" }} }}";
        private string GenerateSize(int size) => $@"""size"": {size}";
        private string GenerateFrom(int from) => $@"""from"": {from}";
    }
}
