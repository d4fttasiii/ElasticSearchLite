using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.BoolHighlight
{
    public interface IBaseBoolHighlightQueryShouldAdded<TPoco, TResult>
        where TPoco : IElasticPoco
    {
        TResult Term(params object[] values);
        TResult Match(object value);
        TResult MatchPhrase(object value);
        TResult MatchPhrasePrefix(object value);
        TResult Range(ElasticRangeOperations op, object value);
    }
}
