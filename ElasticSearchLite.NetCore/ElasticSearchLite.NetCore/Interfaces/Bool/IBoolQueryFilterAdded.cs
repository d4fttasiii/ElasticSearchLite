namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryFilterAdded<TPoco> : IBoolQueryShouldAdded<TPoco> where TPoco : IElasticPoco { }
}
