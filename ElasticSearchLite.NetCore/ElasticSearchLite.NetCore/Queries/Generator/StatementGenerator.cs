using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ElasticSearchLite.NetCore.Queries.Models;

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
                default:
                    throw new Exception("Unknown query type");
            }
        }

        private string GenerateSearchQuery(SearchQuery searchQuery)
        {
            var statement = new StringBuilder($"POST {searchQuery.IndexName}/{searchQuery.TypeName}/_search {{");
            statement.Append(GenerateSources(searchQuery.Fields));

            return statement.ToString();
        }

        private string GenerateDeleteQuery(DeleteQuery deleteQuery)
        {
            var statement = new StringBuilder($"POST {deleteQuery.IndexName}/{deleteQuery.TypeName}/_delete_by_query {{");
            statement.Append(GenerateQuery(deleteQuery));

            return statement.ToString();
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

            if(query.RangeCondition != null)
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
