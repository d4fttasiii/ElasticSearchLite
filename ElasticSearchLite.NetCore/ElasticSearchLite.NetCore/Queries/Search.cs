using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Search
    {
        internal string IndexName { get; }

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
        public SearchQuery<TPoco> ThatReturns<TPoco>() where TPoco : IElasticPoco
        {
            return new SearchQuery<TPoco>(IndexName);
        }

        public abstract class SearchQuery : AbstractQuery
        {
            protected internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            protected internal int Size { get; set; } = 25;
            protected internal int From { get; set; } = 0;
            protected internal Type PocoType { get; set; }

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

            internal SearchQuery(string indexName) : base(indexName)
            {
                PocoType = typeof(TPoco);
            }
            /// <summary>
            /// Term Query finds documents that contain the exact term specified in the inverted index.
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="value">Value which should equal the field content</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Term(string field, object value)
            {
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = field },
                    Value = value
                };

                return Term(condition);
            }
            /// <summary>
            /// Term Query finds documents that contain the exact term specified in the inverted index.
            /// </summary>
            /// <param name="condition"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Term(ElasticTermCodition termCondition)
            {
                TermCondition = termCondition ?? throw new ArgumentNullException(nameof(termCondition));

                return this;
            }
            /// <summary>
            /// Match Query
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Match(string field, object value)
            {
                var condition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = field },
                    Value = value
                };

                return Match(condition);
            }
            /// <summary>
            /// Match Query
            /// </summary>
            /// <param name="condition"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Match(ElasticMatchCodition matchCondition)
            {
                MatchCondition = matchCondition ?? throw new ArgumentNullException(nameof(matchCondition));

                return this;
            }
            /// <summary>
            /// Returns document for a range
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="op">Range operator</param>
            /// <param name="value"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Range(string field, ElasticRangeOperations op, object value)
            {
                var condition = new ElasticRangeCondition
                {
                    Field = new ElasticField { Name = field },
                    Operation = op,
                    Value = value
                };

                return Range(condition);
            }
            /// <summary>
            /// Returns documents for a range condition
            /// </summary>
            /// <param name="rangeCondition"></param>
            /// <returns></returns>
            public IFilteredSearchQuery<TPoco> Range(ElasticRangeCondition rangeCondition)
            {
                RangeCondition = rangeCondition ?? throw new ArgumentNullException(nameof(rangeCondition));

                return this;
            }
            /// <summary>
            /// Orders the documents by this given field
            /// </summary>
            /// <param name="field"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> Sort(string field, ElasticSortOrders sortOrder) => Sort(new ElasticField { Name = field }, sortOrder);
            /// <summary>
            /// Orders the documents by this given field
            /// </summary>
            /// <param name="field"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> Sort(ElasticField field, ElasticSortOrders sortOrder) => Sort(new ElasticSort { Field = field, Order = sortOrder });
            /// <summary>
            /// Orders the documents by this given field
            /// </summary>
            /// <param name="sort"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> Sort(ElasticSort sort)
            {
                SortingFields.Add(sort ?? throw new ArgumentNullException(nameof(sort)));

                return this;
            }
            /// <summary>
            /// Ands another field to the order by fields
            /// </summary>
            /// <param name="field"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> ThenBy(string field, ElasticSortOrders sortOrder) => Sort(field, sortOrder);            
            /// <summary>
            /// Ands another field to the order by fields
            /// </summary>
            /// <param name="field"></param>
            /// <param name="sortOrder"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> ThenBy(ElasticField field, ElasticSortOrders sortOrder) => Sort(field, sortOrder);            
            /// <summary>
            /// Ands another field to the order by fields
            /// </summary>
            /// <param name="sort"></param>
            /// <returns></returns>
            public ISortedSearchQuery<TPoco> ThenBy(ElasticSort sort) => Sort(sort);            
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
