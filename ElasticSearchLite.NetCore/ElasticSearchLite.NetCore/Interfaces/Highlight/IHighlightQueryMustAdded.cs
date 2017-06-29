namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryMustAdded<TPoco> : IHighlightQueryShouldAdded<TPoco> where TPoco: IElasticPoco { }
}
