namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryMustNotAdded<TPoco> : IBoolQueryShouldAdded<TPoco> where TPoco : IElasticPoco { }
}
