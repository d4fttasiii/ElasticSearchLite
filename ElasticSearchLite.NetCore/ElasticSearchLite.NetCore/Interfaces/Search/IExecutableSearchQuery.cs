namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IExecutableSearchQuery<TPoco> : IQuery where TPoco : IElasticPoco
    {

    }
}
