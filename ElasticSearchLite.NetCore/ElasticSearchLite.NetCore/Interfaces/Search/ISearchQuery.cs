using ElasticSearchLite.NetCore.Models;
using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ISearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value which should equal the field content</param>
        /// <returns></returns>
        ITermFilteredSearchQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// Match Query
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// Returns document for a range
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="op">Range operator</param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRangeFilteredSearchQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value);
        /// <summary>
        /// Orders the documents by this given field
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
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
