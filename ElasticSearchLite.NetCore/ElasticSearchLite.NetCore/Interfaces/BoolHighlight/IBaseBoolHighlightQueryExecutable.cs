using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.BoolHighlight
{
    public interface IBaseBoolHighlightQueryExecutable<TPoco, TReturnShould, TReturnMust, TReturnMustNot, TReturnFilter>
        where TPoco : IElasticPoco
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
        TReturnShould Should(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        /// The clause (query) must appear in matching documents and will contribute to the score.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        TReturnMust Must(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        /// The clause (query) must not appear in the matching documents. 
        /// Clauses are executed in filter context meaning that scoring is ignored and clauses are considered for caching. 
        /// Because scoring is ignored, a score of 0 for all documents is returned.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        TReturnMustNot MustNot(Expression<Func<TPoco, object>> propertyExpression);
        /// <summary>
        ///  The clause(query) must appear in matching documents.However unlike must the score of the query will be ignored. 
        ///  Filter clauses are executed in filter context, meaning that scoring is ignored and clauses are considered for caching.
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        TReturnFilter Filter(Expression<Func<TPoco, object>> propertyExpression);
    }
}
