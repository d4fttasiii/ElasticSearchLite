using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryShouldAdded<TPoco> where TPoco : IElasticPoco
    {
        IHighlightQueryExecutable<TPoco> Term(params object[] values);
        IHighlightQueryExecutable<TPoco> Match(object value);
        IHighlightQueryExecutable<TPoco> MatchPhrase(object value);
        IHighlightQueryExecutable<TPoco> MatchPhrasePrefix(object value);
        IHighlightQueryExecutable<TPoco> Range(ElasticRangeOperations op, object value);
    }
}
