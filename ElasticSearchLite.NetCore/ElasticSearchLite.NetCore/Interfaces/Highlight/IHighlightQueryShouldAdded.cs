using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryShouldAdded<TPoco> where TPoco : IElasticPoco
    {
        IHighlightQueryExecutable<TPoco> Match(object value);
        IHighlightQueryExecutable<TPoco> MatchPhrase(object value);
        IHighlightQueryExecutable<TPoco> MatchPhrasePrefix(object value);
        IHighlightQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value);
    }
}
