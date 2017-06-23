using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IShouldAddQuery<TPoco> where TPoco : IElasticPoco
    {
        IExecutableBoolQuery<TPoco> Match(object value);
        IExecutableBoolQuery<TPoco> MatchPhrase(object value);
        IExecutableBoolQuery<TPoco> MatchPhrasePrefix(object value);
        IExecutableBoolQuery<TPoco> Range(ElasticRangeOperations op, object value);
    }
}
