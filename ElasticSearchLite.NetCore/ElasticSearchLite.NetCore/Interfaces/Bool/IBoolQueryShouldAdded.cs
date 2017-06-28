using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryShouldAdded<TPoco> where TPoco : IElasticPoco
    {
        IBoolQueryExecutable<TPoco> Match(object value);
        IBoolQueryExecutable<TPoco> MatchPhrase(object value);
        IBoolQueryExecutable<TPoco> MatchPhrasePrefix(object value);
        IBoolQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value);
    }
}
