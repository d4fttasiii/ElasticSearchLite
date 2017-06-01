using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    /// <summary>
    /// TODOS: match, match_phrase, bool_query, handling sources
    /// </summary>
    public class Search
    {
        private string IndexName { get; }

        protected Search(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentException($"The given index name cannot be empty!"); }

            IndexName = indexName;
        }
        /// <summary>
        /// Sets which index should be searched
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static Search In(string indexName)
        {
            return new Search(indexName);
        }
        /// <summary>
        /// Sets the return Poco type
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public ISearchQuery<TPoco> Return<TPoco>() where TPoco : IElasticPoco
        {
            return new SearchQuery<TPoco>(IndexName);
        }

        public abstract class SearchQuery : AbstractConditionalQuery
        {
            protected internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            protected internal int Size { get; set; } = 25;
            protected internal int From { get; set; } = 0;
            protected SearchQuery(string indexName) : base(indexName) { }
        }

        public sealed class SearchQuery<TPoco> :
            SearchQuery,
            ISearchQuery<TPoco>,
            IFilteredSearchQuery<TPoco>,
            IMatchFilteringQuery<TPoco>,
            ITermFilteredSearchQuery<TPoco>,
            IRangeFilteredSearchQuery<TPoco>,
            ISortedSearchQuery<TPoco>,
            ILimitedResultSearchQuery<TPoco>,
            IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
        {
            internal SearchQuery(string indexName) : base(indexName) { }
            /// <summary>
            /// Term Query finds documents that contain the exact term specified in the inverted index. Doesn't affect the score.
            /// </summary>
            /// <param name="propertyExpression">Field name</param>
            /// <param name="value">Value which should equal the field content</param>
            /// <returns></returns>
            public ITermFilteredSearchQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };
                TermConditions.Add(condition);

                return this;
            }
            /// <summary>
            /// Match Query for full text search.
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public IMatchFilteringQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                MatchCondition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                }; 

                return this;
            }
            /// <summary>
            /// Setting up the match operation
            /// </summary>
            /// <param name="op">Operation type between match tokens (and, or)</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Operator(ElasticOperators op = null)
            {
                MatchCondition.Operation = op ?? ElasticOperators.And;

                return this;
            }
            /// <summary>
            /// match_phrase Query for search whole phrases.
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> MatchPhrase(Expression<Func<TPoco, object>> propertyExpression, string value)
            {
                MatchPhraseCondition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };

                return this;
            }
            /// <summary>
            /// match_phrase_prefix Query for auto_complete like functionality.
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> MatchPhrasePrefix(Expression<Func<TPoco, object>> propertyExpression, string value)
            {
                MatchPhrasePrefixCondition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };

                return this;
            }
            /// <summary>
            /// Multi Match Query - Matching the same value (query) on multiple fields using an OR combination between fields.
            /// </summary>
            /// <param name="value">A value matching multiple fields</param>
            /// <param name="propertyExpressions">Multiple field properties</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> MultiMatch(object value, params Expression<Func<TPoco, object>>[] propertyExpressions)
            {
                MultiMatchConditions = new ElasticMultiMatchCondition
                {
                    Value = value ?? throw new ArgumentNullException(nameof(value)),
                    Fields = propertyExpressions.Select(pe => new ElasticField { Name = GetCorrectPropertyName(pe) })
                };

                return this;
            }
            /// <summary>
            /// Returns document for a numeric range or timeinterval. It doesn't affect the score (filter context).
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="op">Range operator</param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IRangeFilteredSearchQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations rangeOperation, object value)
            {
                var condition = new ElasticRangeCondition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Operation = rangeOperation ?? throw new ArgumentNullException(nameof(rangeOperation)),
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };
                RangeConditions.Add(condition);

                return this;
            }
            /// <summary>
            /// Orders the documents by this given field (default ASC)
            /// NOTE: the default sort is: ["_doc"] to skip scoring and increase performance.
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder = null)
            {
                SortingFields.Add(new ElasticSort
                {
                    Field = new ElasticField
                    {
                        Name = GetCorrectPropertyName(propertyExpression)
                    },
                    Order = sortOrder ?? ElasticSortOrders.Ascending
                });

                return this;
            }
            /// <summary>
            /// Adds another field to the order by fields
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> ThenBy(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder = null) => Sort(propertyExpression, sortOrder);
            /// <summary>
            /// Limits the size of the document result set.
            /// </summary>
            /// <param name="take"></param>
            /// <returns></returns>
            public ILimitedResultSearchQuery<TPoco> Take(int take)
            {
                if (take <= 0) { throw new ArgumentException($"The given value {nameof(take)} should be bigger than 0!"); }
                Size = take;

                return this;
            }
            /// <summary>
            /// Skips a certain number of documents.
            /// </summary>
            /// <param name="skip">Number of documents to skip (Offset). Cannot be more than 10.000!</param>
            /// <returns></returns>
            public IExecutableSearchQuery<TPoco> Skip(int skip)
            {
                if (skip < 0) { throw new ArgumentException($"The given value {nameof(skip)} should be at least 0 or higher!"); }
                if (skip > 10000) { throw new ArgumentException($"The given value {nameof(skip)} should be at less than 10.000!"); }

                From = skip;

                return this;
            }
            /// <summary>
            /// Add another value to the term condition
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public ITermFilteredSearchQuery<TPoco> Or(object value)
            {
                var condition = TermConditions.LastOrDefault();
                TermConditions.Add(new ElasticTermCodition
                {
                    Field = condition.Field,
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                });

                return this;
            }
            /// <summary>
            /// Extends the range condition with a second value (to part in between)
            /// </summary>
            /// <param name="operation"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> And(ElasticRangeOperations operation, object value)
            {
                var condition = RangeConditions.LastOrDefault();
                RangeConditions.Add(new ElasticRangeCondition
                {
                    Field = condition.Field,
                    Operation = operation ?? throw new ArgumentNullException(nameof(operation)),
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                });

                return this;
            }
        }
    }
}
