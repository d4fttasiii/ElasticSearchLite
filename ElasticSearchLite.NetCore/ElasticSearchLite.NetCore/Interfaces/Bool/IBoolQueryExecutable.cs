using System;
using System.Linq.Expressions;
using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryExecutable<TPoco> : IQuery where TPoco : IElasticPoco
    {
        /// <summary>
        /// The clause (query) should appear in the matching document.
        /// If the bool query is in a query context and has a must or filter clause then a document will match the bool query even if none of the should queries match. 
        /// In this case these clauses are only used to influence the score. 
        /// If the bool query is a filter context or has neither must or filter then at least one of the should queries must match a document for it to match the bool query. 
        /// This behavior may be explicitly controlled by settings the minimum_should_match parameter.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IBoolQueryShouldAdded<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        /// The clause (query) must appear in matching documents and will contribute to the score.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IBoolQueryMustAdded<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        /// The clause (query) must not appear in the matching documents. 
        /// Clauses are executed in filter context meaning that scoring is ignored and clauses are considered for caching. 
        /// Because scoring is ignored, a score of 0 for all documents is returned.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IBoolQueryMustNotAdded<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        ///  The clause(query) must appear in matching documents.However unlike must the score of the query will be ignored. 
        ///  Filter clauses are executed in filter context, meaning that scoring is ignored and clauses are considered for caching.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IBoolQueryFilterAdded<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression);
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
        IBoolQueryExecutable<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders order);
    }
}
