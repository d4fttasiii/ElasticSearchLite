using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IRangeFilteredSearchQuery<TPoco> : IFilteredSearchQuery<TPoco>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Extends the range condition
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> And(ElasticRangeOperations operation, object value);
    }
}
