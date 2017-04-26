using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ISortedSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        ISortedSearchQuery<TPoco> ThenBy(ElasticField field, ElasticSortOrders sortOrder);
        ISortedSearchQuery<TPoco> ThenBy(string field, ElasticSortOrders sortOrder);
        ILimitedResultSearchQuery<TPoco> Take(int take);
    }
}
