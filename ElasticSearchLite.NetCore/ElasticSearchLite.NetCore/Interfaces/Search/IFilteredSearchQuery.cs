using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IFilteredSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        ISortedSearchQuery<TPoco> Sort(ElasticField field, ElasticSortOrders sortOrder);
        ISortedSearchQuery<TPoco> Sort(string field, ElasticSortOrders sortOrder);

        ILimitedResultSearchQuery<TPoco> Take(int take);
    }
}
