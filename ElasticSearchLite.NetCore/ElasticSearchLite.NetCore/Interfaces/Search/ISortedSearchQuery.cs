using System;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ISortedSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Ands another field to the order by fields
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        ISortedSearchQuery<TPoco> ThenBy(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder);
        /// <summary>
        /// Limits the size of the document result set.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        ILimitedResultSearchQuery<TPoco> Take(int take);
    }
}
