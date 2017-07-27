using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryShouldAdded<TPoco> where TPoco : IElasticPoco
    {
        IBoolQueryExecutable<TPoco> Term(params object[] values);
        IBoolQueryExecutable<TPoco> Match(object value);
        IBoolQueryExecutable<TPoco> MatchPhrase(object value);
        IBoolQueryExecutable<TPoco> MatchPhrasePrefix(object value);
        IBoolQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value);
    }
}
