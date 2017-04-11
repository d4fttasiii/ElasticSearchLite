using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ElasticSearchLite.NetCore.Queries.Models;
using System.Reflection;
using Newtonsoft.Json;

namespace ElasticSearchLite.NetCore.Queries.Generator
{
    public class StatementGenerator : IStatementGenerator
    {
        public string Generate(IQuery query)
        {
            switch (query)
            {
                case SearchQuery searchQuery:
                    return GenerateSearchQuery(searchQuery);
                case DeleteQuery deleteQuery:
                    return GenerateDeleteQuery(deleteQuery);
                case InsertQuery insertQuery:
                    return GenerateInsertQuery(insertQuery);
                case UpdateQuery updateQuery:
                    return GenerateUpdateQuery(updateQuery);
                default:
                    throw new Exception("Unknown query type");
            }
        }

        private string GenerateSearchQuery(SearchQuery searchQuery)
        {
            var statement = new StringBuilder("{");
            statement.Append(GenerateSources(searchQuery.Fields));
            statement.Append(GenerateQuery(searchQuery));

            return statement.Append("}").ToString();
        }

        private string GenerateDeleteQuery(DeleteQuery deleteQuery)
        {
            var statement = new StringBuilder($"POST {deleteQuery.IndexName}/{deleteQuery.TypeName}/_delete_by_query {{");
            statement.Append(GenerateQuery(deleteQuery));

            return statement.ToString();
        }

        private string GenerateInsertQuery(InsertQuery insertQuery)
        {
            var statement = new StringBuilder($"PUT {insertQuery.IndexName}/{insertQuery.TypeName} {{");
            var properties = insertQuery.Poco.GetType().GetProperties();
            var propertiesAsJson = properties.Select(p => $@"""{p.Name}"": ""{p.GetValue(insertQuery.Poco)}""");

            // JsonConvert.SerializeObject(insertQuery.Poco);
            statement.Append(string.Join(",", propertiesAsJson));
            statement.Append("}");

            return statement.Append($"}}").ToString();
        }

        private string GenerateUpdateQuery(UpdateQuery updateQuery)
        {
            var statement = new StringBuilder($"POST {updateQuery.IndexName}/{updateQuery.TypeName}/_update_by_query {{");

            return statement.Append("}").ToString();
        }

        private string GenerateSources(List<ElasticField> fields)
        {
            if (fields.Any())
            {
                var includedFields = string.Join(",", fields.Select(f => $"\"{f.Name}\""));

                return $@"""_source"": {{ ""includes"": [{includedFields}] }}";
            }

            return @"""_source"": true";
        }

        private string GenerateQuery(AbstractQuery query)
        {
            if (query.IsMatchAll)
            {
                return string.Empty;
            }

            if (query.MatchCondition != null)
            {
                return $@", ""query"": {{ {GenerateMatch(query.MatchCondition)} }}";
            }

            if (query.TermCondition != null)
            {
                return $@", ""query"": {{ {GenerateTerm(query.TermCondition)} }}";
            }

            if (query.RangeCondition != null)
            {
                return $@", ""query"": {{ {GenerateRange(query.RangeCondition)} }}";
            }

            return string.Empty;
        }

        private string GenerateMatch(ElasticCodition condition)
        {
            return $@"""match"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        }

        private string GenerateTerm(ElasticCodition condition)
        {
            return $@"""term"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        }

        private string GenerateRange(ElasticRangeCondition condition)
        {
            return $@"""range"": {{""{condition.Field.Name}"": {{""{condition.Operation.Name}"" : ""{condition.Value}"" }} }}";
        }
    }
}
