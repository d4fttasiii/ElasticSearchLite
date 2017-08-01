using System;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Interfaces.BoolHighlight;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryExecutable<TPoco> : 
        IBaseBoolHighlightQueryExecutable<TPoco, IBoolQueryShouldAdded<TPoco>, IBoolQueryMustAdded<TPoco>, IBoolQueryMustNotAdded<TPoco>, IBoolQueryFilterAdded<TPoco>>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Defining _sources 
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IBoolQueryExecutable<TPoco> Sources(params Expression<Func<TPoco, object>>[] propertyExpressions);
        /// <summary>
        /// Sets minimum_should_match parameter.
        /// </summary>
        /// <param name="minimumNumber">Bigger than 0</param>
        /// <returns></returns>
        IBoolQueryExecutable<TPoco> ShouldMatchAtLeast(int minimumNumber);
        /// <summary>
        /// Limits the size of the document result set.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        IBoolQueryExecutable<TPoco> Take(int take);
        /// <summary>
        /// Skips a certain number of documents.
        /// </summary>
        /// <param name="skip">Number of documents to skip (Offset).</param>
        /// <returns></returns>
        IBoolQueryExecutable<TPoco> Skip(int skip);
        /// <summary>
        /// Sorting the document by the given field.
        /// </summary>
        /// <param name="propertyExpression">Column selected for sort.</param>
        /// <returns></returns>
        IBoolQuerySortAdded<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression);
    }
}
