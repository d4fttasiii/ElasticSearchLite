using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            var statement = new StringBuilder("POST _search {");
            statement.Append(GenerateSources(searchQuery.Fields));
            return statement.ToString();
        }

        private string GenerateDeleteQuery(DeleteQuery deleteQuery)
        {
            throw new NotImplementedException();
        }

        private string GenerateSources(List<IElasticField> fields)
        {
            if (fields.Any())
            {
                var includedFields = string.Join(",", fields.Select(f => $"\"{f.Name}\""));

                return $@"""_source"": {{ ""includes"": [{includedFields}] }}";
            }

            return @"""_source"": true";
        }

        private string GenerateQuery(SearchQuery searchQuery)
        {
            if (searchQuery.IsMatchAll)
            {
                return string.Empty;
            }

            if (searchQuery.MatchCondition != null)
            {
                return $@", ""query"": {{ {GenerateMatch(searchQuery.MatchCondition)} }}";
            }

            if (searchQuery.TermCondition != null)
            {
                return $@", ""query"": {{ {GenerateTerm(searchQuery.TermCondition)} }}";
            }

            return $@"""query"": {{ {GenerateMultiMatch(searchQuery.MatchConditions)} }}";
        }

        private string GenerateMatch(ElasticCodition condition)
        {
            return $@"""match"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        }

        private string GenerateTerm(ElasticCodition condition)
        {
            return $@"""term"": {{ ""{condition.Field.Name}"" : ""{condition.Value}"" }}";
        }
        
        private string GenerateMultiMatch(List<ElasticCodition> conditions)
        {
            // TODO: Multi match
            return "";
        }
    }
}
