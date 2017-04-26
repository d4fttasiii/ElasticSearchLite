namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ILimitedResultSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        IExecutableSearchQuery<TPoco> Skip(int skip);
    }
}
