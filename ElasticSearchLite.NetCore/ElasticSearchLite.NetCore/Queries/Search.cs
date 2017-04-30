using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ElasticSearchLite.NetCore.Queries
{
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
        public SearchQuery<TPoco> Return<TPoco>() where TPoco : IElasticPoco
        {
            return new SearchQuery<TPoco>(IndexName);
        }

        public abstract class SearchQuery : AbstractQuery
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
            ISortedSearchQuery<TPoco>,
            ILimitedResultSearchQuery<TPoco>,
            IExecutableSearchQuery<TPoco> where TPoco : IElasticPoco
        {

            internal SearchQuery(string indexName) : base(indexName) { }
            /// <summary>
            /// Term Query finds documents that contain the exact term specified in the inverted index.
            /// </summary>
            /// <param name="propertyExpression">Field name</param>
            /// <param name="value">Value which should equal the field content</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = propertyInfo.Name },
                    Value = value
                };
                TermCondition = condition;

                return this;
            }
            /// <summary>
            /// Match Query
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
                var condition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = propertyInfo.Name },
                    Value = value
                };
                MatchCondition = condition;

                return this;
            }
            /// <summary>
            /// Returns document for a range
            /// </summary>
            /// <param name="propertyExpression">Field property</param>
            /// <param name="op">Range operator</param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations op, object value)
            {
                var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
                var condition = new ElasticRangeCondition
                {
                    Field = new ElasticField { Name = propertyInfo.Name },
                    Operation = op,
                    Value = value
                };
                RangeCondition = condition;

                return this;
            }
            /// <summary>
            /// Orders the documents by this given field
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> Sort(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder)
            {
                var propertyInfo = ((MemberExpression)propertyExpression.Body).Member as PropertyInfo;
                SortingFields.Add(new ElasticSort
                {
                    Field = new ElasticField
                    {
                        Name = propertyInfo.Name
                    },
                    Order = sortOrder

                });

                return this;
            }
            /// <summary>
            /// Ands another field to the order by fields
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> ThenBy(Expression<Func<TPoco, object>> propertyExpression, ElasticSortOrders sortOrder) => Sort(propertyExpression, sortOrder);
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
            /// <param name="skip">Number of documents to skip (Offset).</param>
            /// <returns></returns>
            public IExecutableSearchQuery<TPoco> Skip(int skip)
            {
                if (skip < 0) { throw new ArgumentException($"The given value {nameof(skip)} should be at least 0 or higher!"); }
                From = skip;

                return this;
            }
        }
    }
}
