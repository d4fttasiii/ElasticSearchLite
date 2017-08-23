using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Models.Conditions;
using ElasticSearchLite.NetCore.Models.Enums;
using ElasticSearchLite.NetCore.Queries.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ElasticSearchLite.NetCore.Queries.Bool;
using static ElasticSearchLite.NetCore.Queries.Create;
using static ElasticSearchLite.NetCore.Queries.Delete;
using static ElasticSearchLite.NetCore.Queries.Highlight;
using static ElasticSearchLite.NetCore.Queries.MGet;
using static ElasticSearchLite.NetCore.Queries.Search;

namespace ElasticSearchLite.NetCore.Queries.Generator
{
    public class StatementFactory : IStatementFactory
    {
        public NamingStrategy NamingStrategy { get; set; } = new CamelCaseNamingStrategy();
        public DefaultContractResolver ContractResolver { get; set; } = new JsonElasticPropertyResolver();

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
                case CreateQuery createQuery:
                    return GenerateCreateQuery(createQuery);
                case HighlightQuery highlightQuery:
                    return GenerateHighlightQuery(highlightQuery);
                case BoolQuery boolQuery:
                    return GenerateBoolQuery(boolQuery);
                case MultiGetQuery multiGetQuery:
                    return GenerateMultiGetQuery(multiGetQuery);
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
            statementParts.Add($@"""version"": true");
            statementParts.Add(GenerateSort(searchQuery.SortingFields));

            return $"{{ {string.Join(",", statementParts)} }}";
        }
        private string GenerateBoolQuery(BoolQuery boolQuery)
        {
            var fieldNames = string.Join(", ",boolQuery.SourceFields.Select(f => $@"""{GetName(f.Name)}"""));
            var parts = new List<string>
            {
                $@"""_source"": [{(fieldNames.Any() ? fieldNames : @"""*""")}]",
                $@"""version"": true",
                $@"""query"": {{ ""bool"": {{ {GenerateBoolQueryConditions(boolQuery.Conditions, boolQuery.MinimumNumberShouldMatch)} }} }}",
                GenerateSort(boolQuery.SortingFields),
                GenerateSize(boolQuery.Size),
                GenerateFrom(boolQuery.From)
            };

            return $@"{{ {string.Join(",", parts)} }}";
        }
        private string GenerateHighlightQuery(HighlightQuery highlightQuery)
        {
            var parts = new List<string>
            {
                $@"""_source"": false",
                $@"""query"": {{ ""bool"": {{ {GenerateBoolQueryConditions(highlightQuery.Conditions, highlightQuery.MinimumNumberShouldMatch)} }} }}",
                GenerateHighlight(highlightQuery.Highlight, highlightQuery.NumberOfFragements, highlightQuery.FragmentSize),
                GenerateSize(highlightQuery.Size),
                GenerateFrom(highlightQuery.From)
            };

            return $@"{{ {string.Join(",", parts)} }}";
        }
        private string GenerateDeleteQuery(DeleteQuery deleteQuery)
        {
            return $"{{ {GenerateQuery(deleteQuery)} }}";
        }
        private string GenerateInsertQuery(Index indexQuery) => $"{GenerateFieldMapping(indexQuery.Poco)}";
        private string GenerateUpdateQuery(Update updateQuery)
        {
            return $"{{ {GenerateDocument(updateQuery.Poco)} }}";
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
        private string GenerateBoolQueryConditions(Dictionary<ElasticBoolQueryOccurrences, List<IElasticCondition>> conditions, int minimumShouldMatch)
        {
            var builder = new List<string>();

            foreach (var condition in conditions.Where(c => c.Value.Count > 0))
            {
                var queryConditions = condition.Value.Select(c => $" {{ {GenerateCondition(c)} }} ");

                builder.Add($@" ""{condition.Key.Name}"": [{string.Join(",", queryConditions)}] ");
            }

            builder.Add($@" ""minimum_should_match"": {minimumShouldMatch} ");

            return string.Join(",", builder);
        }
        private string GenerateMultiGetQuery(MultiGetQuery multiGetQuery)
        {
            var docIds = multiGetQuery.Ids.Select(id => $@"{{ ""{ElasticFields.Id.Name}"": {EscapeValue(id)} }}");

            return $@"{{ ""docs"": [ {string.Join(",", docIds)} ] }}";
        }
        private string GenerateCondition(IElasticCondition condition)
        {
            switch (condition)
            {
                case ElasticMatchCodition matchCondition:
                    return GenerateMatch(matchCondition);
                case ElasticMatchPhraseCondition matchPhraseCondition:
                    return GenerateMatchPhrase(matchPhraseCondition);
                case ElasticMatchPhrasePrefixCondition matchPhrasePrefixCondition:
                    return GenerateMatchPhrasePrefixPhrase(matchPhrasePrefixCondition);
                case ElasticRangeCondition rangeCondition:
                    return GenerateRange(new List<ElasticRangeCondition> { rangeCondition });
                case ElasticTermCodition termCondition:
                    return GenerateTerms(termCondition);
                default:
                    throw new Exception("Unknown condition type");
            }
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
                        statement.AppendLine($"{GenerateFieldMapping(poco)}");
                        break;
                    case "update":
                        statement.AppendLine($"{{ {GenerateDocument(poco)} }}");
                        break;
                }
            }

            return statement.ToString();
        }
        private string GenerateCreateQuery(CreateQuery createQuery)
        {
            return $@"{{ ""settings"": {{ 
                ""number_of_shards"": {createQuery.NumberOfShards}, 
                ""number_of_replicas"": {createQuery.NumberOfReplicas},
                ""{createQuery.TypeName}"": {{ ""_all"": {EscapeValue(createQuery.AllFieldEnabled)}, 
                {string.Join(",", createQuery.Mapping.Select(m => $@"""{m.Name}"" : {{ 
                    ""type"": ""{m.FieldDataType.Name}"" 
                    {(m.Analyzer != null ? $@",""analyzer"": ""{m.Analyzer.Name}""" : "")} }}"))} }} }} }}";
        }
        private string GenerateDocument(IElasticPoco poco) => $@"""doc"": {GenerateFieldMapping(poco)}";
        private string GenerateFieldMapping(IElasticPoco poco) => JsonConvert.SerializeObject(poco, new JsonSerializerSettings { ContractResolver = ContractResolver });
        private string GenerateQuery(AbstractConditionalQuery query)
        {
            if (query.MatchCondition != null)
            {
                return $@"""query"": {{ {GenerateMatch(query.MatchCondition)} }}";
            }

            if (query.MatchPhraseCondition != null)
            {
                return $@"""query"": {{ {GenerateMatchPhrase(query.MatchPhraseCondition)} }}";
            }

            if (query.MatchPhrasePrefixCondition != null)
            {
                return $@"""query"": {{ {GenerateMatchPhrasePrefixPhrase(query.MatchPhrasePrefixCondition)} }}";
            }

            if (query.MultiMatchConditions != null)
            {
                return $@"""query"": {{ {GenerateMultiMatch(query.MultiMatchConditions)} }}";
            }

            if (query.TermCondition != null)
            {
                return $@"""query"": {{ {GenerateTerms(query.TermCondition)} }}";
            }

            if (query.RangeConditions.Count > 0)
            {
                return $@"""query"": {{ {GenerateRange(query.RangeConditions)} }}";
            }

            return string.Empty;
        }
        private string GetName(string name, bool useKeywordField = false)
        {
            var elasticFieldName = string.Join(".", name.Split('.').Select(p => NamingStrategy.GetPropertyName(p, false)));
            if (useKeywordField)
            {
                elasticFieldName = $"{elasticFieldName}.keyword";
            }

            return elasticFieldName;
        }
        private string EscapeValue(object value) => JsonConvert.SerializeObject(value);
        private string GenerateMatch(ElasticMatchCodition condition) => $@"""match"": {{ ""{GetName(condition.Field.Name)}"" : {{ ""query"": {EscapeValue(condition.Value)}, ""operator"": ""{condition.Operation.Name}"" }} }}";
        private string GenerateMatchPhrase(ElasticMatchPhraseCondition condition) => $@"""match_phrase"": {{ ""{GetName(condition.Field.Name)}"" : {{ ""query"": {EscapeValue(condition.Value)}, ""slop"": {condition.Slop} }} }}";
        private string GenerateMatchPhrasePrefixPhrase(ElasticMatchPhrasePrefixCondition condition) => $@"""match_phrase_prefix"": {{ ""{GetName(condition.Field.Name)}"" : {EscapeValue(condition.Value)} }}";
        private string GenerateMultiMatch(ElasticMultiMatchCondition condition) => $@"""multi_match"": {{ ""query"": {EscapeValue(condition.Value)}, ""fields"": [{string.Join(",", condition.Fields.Select(cf => $@"""{GetName(cf.Name)}"""))}] }}";
        private string GenerateTerms(ElasticTermCodition condition)
        {
            var fieldName = string.Join(".", condition.Field.Name.Split('.').Select(name => GetName(name)));
            var escapedValues = EscapeValue(condition.Values);

            return $@"""terms"": {{ ""{fieldName}"" : {escapedValues} }}";
        }
        private string GenerateRange(List<ElasticRangeCondition> conditions) => $@"""range"": {{""{GetName(conditions.First().Field.Name)}"": {{ {string.Join(",", conditions.Select(c => $@" ""{c.Operation.Name}"": {EscapeValue(c.Value)}"))} }} }}";
        private string GenerateSize(int size) => $@"""size"": {size}";
        private string GenerateFrom(int from) => $@"""from"": {from}";
        // TODO: search_after implementation
        private string GenerateSort(List<ElasticSort> sortingFields)
        {
            if (sortingFields != null && sortingFields.Count > 0)
            {
                return $@"""sort"": [{string.Join(",", sortingFields.Select(sf => $@"{{""{GetName(sf.Field.Name, sf.Field.UseKeywordField)}"": ""{sf.Order.Name}""}}"))}]";
            }

            return $@"""sort"": [""_doc""]";
        }
        private string GenerateHighlight(ElasticHighlight highlight, int numberOfFragments, int fragmentSize)
        {
            var fields = highlight.HighlightedFields.Select(f => $@" ""{GetName(f.Name)}"": {{ ""number_of_fragments"": {numberOfFragments}, ""fragment_size"": {fragmentSize}}} ");

            return $@" ""highlight"": {{ ""pre_tags"": [ ""{highlight.PreTag}"" ], ""post_tags"": [ ""{highlight.PostTag}"" ], ""fields"": {{ {string.Join(",", fields)} }} }} ";
        }
    }
}
