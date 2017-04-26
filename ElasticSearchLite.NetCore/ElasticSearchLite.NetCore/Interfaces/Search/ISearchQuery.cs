using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ISearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        IFilteredSearchQuery<TPoco> Term(string field, object value);
        IFilteredSearchQuery<TPoco> Term(ElasticTermCodition condition);
        IFilteredSearchQuery<TPoco> Match(string field, object value);
        IFilteredSearchQuery<TPoco> Match(ElasticMatchCodition condition);
        IFilteredSearchQuery<TPoco> Range(string field, ElasticRangeOperations operation, object value);
        IFilteredSearchQuery<TPoco> Range(ElasticRangeCondition condition);
        ISortedSearchQuery<TPoco> Sort(string field, ElasticSortOrders sortOrder);
        ISortedSearchQuery<TPoco> Sort(ElasticField field, ElasticSortOrders sortOrder);
        ILimitedResultSearchQuery<TPoco> Take(int take);
    }
}
