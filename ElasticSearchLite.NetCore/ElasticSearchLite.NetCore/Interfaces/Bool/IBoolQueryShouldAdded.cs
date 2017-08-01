using ElasticSearchLite.NetCore.Interfaces.BoolHighlight;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryShouldAdded<TPoco> : IBaseBoolHighlightQueryShouldAdded<TPoco, IBoolQueryExecutable<TPoco>>
        where TPoco : IElasticPoco
    {
    }
    
}
