using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IRangeFilteredSearchQuery<TPoco> : IFilteredSearchQuery<TPoco>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Extends the range condition with a second value (to part in between)
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> And(ElasticRangeOperations operation, object value);
    }
}
