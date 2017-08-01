using ElasticSearchLite.NetCore.Interfaces.BoolHighlight;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryShouldAdded<TPoco> : IBaseBoolHighlightQueryShouldAdded<TPoco, IHighlightQueryExecutable<TPoco>>
        where TPoco : IElasticPoco
    { }
}
