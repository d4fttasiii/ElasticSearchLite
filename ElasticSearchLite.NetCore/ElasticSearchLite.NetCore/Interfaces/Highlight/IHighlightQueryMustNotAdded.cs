namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryMustNotAdded<TPoco> : IHighlightQueryShouldAdded<TPoco> where TPoco : IElasticPoco { }
}
