using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System;
using System.Text;

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
                default:
                    throw new Exception("Unknown query type");
            }
        }

        private string GenerateSearchQuery(SearchQuery searchQuery)
        {
            var statement = new StringBuilder("POST _search { \"query\": {");
            foreach (var term in searchQuery.Terms)
            {
                var t = $"\"{term.Field.Name}\" : \"{term.Value}\"";
            }


            /*$"POST _search
{
                "query": {
                    "term" : { "user" : "Kimchy" }
                }
            }
            ";*/

            return statement.ToString();
        }

        private string GenerateCondition(ElasticCodition condition)
        {
            return $"{{ \"{condition.Field.Name}\" : \"{condition.Value}\" }}";
        }
    }
}
