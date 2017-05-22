using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;
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
                case Upsert upsertQuery:
                    return GenerateUpsertQuery(upsertQuery);
                case Bulk bulkQuery:
                    return GenerateBulkQuery(bulkQuery);
                default:
                    throw new Exception("Unknown query type");
            }
        }

        private string GenerateSearchQuery(SearchQuery searchQuery)
        {
            var statementParts = new List<string>();
            var query = GenerateQuery(searchQuery);

            if (!string.IsNullOrEmpty(query)) { statementParts.Add(query); }
            if (searchQuery.Size != 0) { statementParts.Add(GenerateSize(searchQuery.Size)); }
            if (searchQuery.From != 0) { statementParts.Add(GenerateFrom(searchQuery.From)); }
            if (searchQuery.SortingFields != null && searchQuery.SortingFields.Count > 0) { statementParts.Add(GenerateSort(searchQuery.SortingFields)); }
          
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

        private string GenerateUpsertQuery(Upsert upsertQuery)
        {
            var statementParts = new List<string>
            {
                GenerateDocument(upsertQuery.Poco),
                $@"""doc_as_upsert"": true"
            };

            return $"{{ {string.Join(",", statementParts)} }}";
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

            if (query.TermConditions.Count > 0)
            {
                return $@"""query"": {{ {GenerateTerms(query.TermConditions)} }}";
            }

            if (query.RangeConditions.Count > 0)
            {
                return $@"""query"": {{ {GenerateRange(query.RangeConditions)} }}";
            }

            return string.Empty;
        }

        private string EscapeValue(object value) => JsonConvert.SerializeObject(value);
        private string GenerateMatch(ElasticMatchCodition condition) => $@"""match"": {{ ""{condition.Field.Name}"" : {EscapeValue(condition.Value)} }}";
        private string GenerateTerms(List<ElasticTermCodition> conditions) => $@"""terms"": {{ ""{conditions.First().Field.Name}"" : {EscapeValue(conditions.Select(c => c.Value).ToArray())} }}";
        private string GenerateRange(List<ElasticRangeCondition> conditions) => $@"""range"": {{""{conditions.First().Field.Name}"": {{ {string.Join(",", conditions.Select(c => $@" ""{c.Operation.Name}"": {EscapeValue(c.Value)}"))} }} }}";
        private string GenerateSize(int size) => $@"""size"": {size}";
        private string GenerateFrom(int from) => $@"""from"": {from}";
        private string GenerateSort(List<ElasticSort> sortingFields) => $@"""sort"": [{string.Join(",", sortingFields.Select(sf => $@"{{""{sf.Field.Name}"": ""{sf.Order.Name}""}}"))}]";
    }
}
