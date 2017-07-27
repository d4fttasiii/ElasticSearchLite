using System;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface IFilteredSearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Orders the documents by this given field (default ASC)
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        ISortedSearchQuery<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder);
        /// <summary>
        /// Limits the size of the document result set.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        ILimitedResultSearchQuery<TPoco> Take(int take);
    }
}
