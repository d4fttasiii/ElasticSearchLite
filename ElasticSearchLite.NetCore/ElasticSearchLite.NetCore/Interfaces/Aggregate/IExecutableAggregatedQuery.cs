namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IExecutableAggregatedQuery<TPoco> : IQuery
        where TPoco : IElasticPoco
    {
    }
}
