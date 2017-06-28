namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryMustAdded<TPoco> : IBoolQueryShouldAdded<TPoco> where TPoco: IElasticPoco { }
}
