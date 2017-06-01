using ElasticSearchLite.NetCore.Models;
using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ISearchQuery<TPoco> : IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
    {
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index. Doesn't affect the score.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value which should equal the field content</param>
        /// <returns></returns>
        ITermFilteredSearchQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// Match Query for full text search.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IMatchFilteringQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// match_phrase Query for search whole phrases.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> MatchPhrase(Expression<Func<TPoco, object>> propertyExpression, string value);
        /// <summary>
        /// match_phrase_prefix Query for auto_complete like functionality.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> MatchPhrasePrefix(Expression<Func<TPoco, object>> propertyExpression, string value);
        /// <summary>
        /// Multi Match Query - Matching the same value (query) on multiple fields using an OR combination between fields.
        /// </summary>
        /// <param name="value">A value matching multiple fields</param>
        /// <param name="propertyExpressions">Multiple field properties</param>
        /// <returns></returns>
        IFilteredSearchQuery<TPoco> MultiMatch(object value, params Expression<Func<TPoco, object>>[] propertyExpressions);
        /// <summary>
        /// Returns document for a numeric range or timeinterval. It doesn't affect the score (filter context).
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="op">Range operator</param>
        /// <param name="value"></param>
        /// <returns></returns>
        IRangeFilteredSearchQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value);
        /// <summary>
        /// Orders the documents by this given field (default ASC)
        /// NOTE: the default sort is: ["_doc"] to skip scoring and increase performance.
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
