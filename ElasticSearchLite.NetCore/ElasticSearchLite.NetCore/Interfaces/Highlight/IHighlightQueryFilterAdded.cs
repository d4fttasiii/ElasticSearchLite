namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryFilterAdded<TPoco> : IHighlightQueryShouldAdded<TPoco> where TPoco : IElasticPoco { }
}
