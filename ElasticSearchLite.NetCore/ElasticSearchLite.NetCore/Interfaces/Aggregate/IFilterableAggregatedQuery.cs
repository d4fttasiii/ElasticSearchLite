using ElasticSearchLite.NetCore.Models.Enums;
using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IFilterableAggregatedQuery<TPoco>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index. Doesn't affect the score.
        /// </summary>
        /// <param name="propertyExpression">Field name</param>
        /// <param name="value">Value which should equal the field content</param>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// he most simple query, which matches all documents, giving them all a _score of 1.0.
        /// </summary>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> MatchAll();
        /// <summary>
        /// Match Query for full text search.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value);
        /// <summary>
        /// match_phrase Query for search whole phrases.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> MatchPhrase(Expression<Func<TPoco, object>> propertyExpression, string value);
        /// <summary>
        /// match_phrase_prefix Query for auto_complete like functionality.
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> MatchPhrasePrefix(Expression<Func<TPoco, object>> propertyExpression, string value);
        /// <summary>
        /// Returns document for a numeric range or timeinterval. It doesn't affect the score (filter context).
        /// </summary>
        /// <param name="propertyExpression">Field property</param>
        /// <param name="op">Range operator</param>
        /// <param name="value"></param>
        /// <returns></returns>
        IFilteredAggregatedQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value);
    }
}
