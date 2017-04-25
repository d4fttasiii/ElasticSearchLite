using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Search
    {
        internal string IndexName { get; }

        protected Search(string indexName)
        {
            if (string.IsNullOrEmpty(IndexName)) { throw new ArgumentNullException(nameof(indexName)); }

            IndexName = indexName;
        }

        public static Search In(string indexName)
        {
            return new Search(indexName);
        }

        public SearchQuery<TPoco> ThatReturns<TPoco>() where TPoco : IElasticPoco
        {
            return new SearchQuery<TPoco>(IndexName);
        }

        public sealed class SearchQuery<TPoco> where TPoco : IElasticPoco
        {
            internal Type PocoType { get; }
            internal string IndexName { get; }
            internal List<ElasticField> Fields { get; } = new List<ElasticField>();

            internal SearchQuery(string indexName)
            {
                IndexName = indexName;
                PocoType = typeof(TPoco);
            }

            /// <summary>
            /// Include certain fields to a query
            /// </summary>
            /// <param name="incluededFields"></param>
            /// <returns></returns>
            public SearchQuery<TPoco> Include(params ElasticField[] incluededFields)
            {
                var include = incluededFields.Where(inc => !Fields.Select(f => f.Name).Contains(inc.Name));
                Fields.AddRange(include);

                return this;
            }
            /// <summary>
            /// Exclude certain fields from a query
            /// </summary>
            /// <param name="excludeFields"></param>
            /// <returns></returns>
            public SearchQuery<TPoco> Exclude(params ElasticField[] excludeFields)
            {
                var exclude = excludeFields.Where(ex => !Fields.Select(f => f.Name).Contains(ex.Name));
                Fields.Clear();
                Fields.AddRange(exclude);

                return this;
            }

            /// <summary>
            /// Term Query finds documents that contain the exact term specified in the inverted index.
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="value">Value which should equal the field content</param>
            /// <returns></returns>
            public FilteredSearchQuery<TPoco> Term(string field, object value)
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
            public FilteredSearchQuery<TPoco> Term(ElasticTermCodition condition)
            {
                return new FilteredSearchQuery<TPoco>(this, condition);
            }
            /// <summary>
            /// Match Query
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="value">Value matching the field</param>
            /// <returns></returns>
            public FilteredSearchQuery<TPoco> Match(string field, object value)
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
            public FilteredSearchQuery<TPoco> Match(ElasticMatchCodition condition)
            {
                return new FilteredSearchQuery<TPoco>(this, condition);
            }
            /// <summary>
            /// Returns document for a range
            /// </summary>
            /// <param name="field">Field name</param>
            /// <param name="op">Range operator</param>
            /// <param name="value"></param>
            /// <returns></returns>
            public FilteredSearchQuery<TPoco> Range(string field, ElasticRangeOperations op, object value)
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
            /// Returns document for a range
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            public FilteredSearchQuery<TPoco> Range(ElasticRangeCondition condition)
            {
                return new FilteredSearchQuery<TPoco>(this, condition);
            }
            public SortedSearchQuery<TPoco> Sort(string field, ElasticSortOrders sortOrder)
            {
                return Sort(new ElasticField { Name = field }, sortOrder);
            }
            public SortedSearchQuery<TPoco> Sort(ElasticField field, ElasticSortOrders sortOrder)
            {
                return new SortedSearchQuery<TPoco>(this, new ElasticSort { Field = field, Order = sortOrder });
            }

            /// <summary>
            /// Limits the size of the document result set.
            /// </summary>
            /// <param name="take"></param>
            /// <returns></returns>
            public LimitedSearchQuery<TPoco> Take(int take)
            {
                return new LimitedSearchQuery<TPoco>(this, take);
            }
        }

        public sealed class FilteredSearchQuery<TPoco> where TPoco : IElasticPoco
        {
            internal Type PocoType { get; }
            internal string IndexName { get; }
            internal List<ElasticField> Fields { get; } = new List<ElasticField>();
            internal ElasticTermCodition TermCondition { get; set; }
            internal ElasticMatchCodition MatchCondition { get; set; }
            internal ElasticRangeCondition RangeCondition { get; set; }

            internal FilteredSearchQuery(SearchQuery<TPoco> searchQuery, ElasticTermCodition termCondition)
            {
                PocoType = searchQuery.PocoType;
                Fields = searchQuery.Fields;
                IndexName = searchQuery.IndexName;
                TermCondition = termCondition;
            }

            internal FilteredSearchQuery(SearchQuery<TPoco> searchQuery, ElasticMatchCodition matchCondition)
            {
                PocoType = searchQuery.PocoType;
                Fields = searchQuery.Fields;
                IndexName = searchQuery.IndexName;
                MatchCondition = matchCondition;
            }

            internal FilteredSearchQuery(SearchQuery<TPoco> searchQuery, ElasticRangeCondition rangeCondition)
            {
                PocoType = searchQuery.PocoType;
                Fields = searchQuery.Fields;
                IndexName = searchQuery.IndexName;
                RangeCondition = rangeCondition;
            }

            public SortedSearchQuery<TPoco> Sort(string field, ElasticSortOrders sortOrder)
            {
                return Sort(new ElasticField { Name = field }, sortOrder);
            }
            public SortedSearchQuery<TPoco> Sort(ElasticField field, ElasticSortOrders sortOrder)
            {
                return new SortedSearchQuery<TPoco>(this, new ElasticSort { Field = field, Order = sortOrder });
            }

            /// <summary>
            /// Limits the size of the document result set.
            /// </summary>
            /// <param name="take"></param>
            /// <returns></returns>
            public LimitedSearchQuery<TPoco> Take(int take)
            {
                return new LimitedSearchQuery<TPoco>(this, take);
            }
        }

        public sealed class SortedSearchQuery<TPoco> where TPoco : IElasticPoco
        {
            internal Type PocoType { get; }
            internal string IndexName { get; }
            internal List<ElasticField> Fields { get; } = new List<ElasticField>();
            internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            internal ElasticTermCodition TermCondition { get; set; }
            internal ElasticMatchCodition MatchCondition { get; set; }
            internal ElasticRangeCondition RangeCondition { get; set; }

            internal SortedSearchQuery(SearchQuery<TPoco> searchQuery, ElasticSort sort)
            {
                PocoType = searchQuery.PocoType;
                IndexName = searchQuery.IndexName;
                Fields = searchQuery.Fields;
            }

            internal SortedSearchQuery(FilteredSearchQuery<TPoco> filteredSearchQuery, ElasticSort sort)
            {
                PocoType = filteredSearchQuery.PocoType;
                IndexName = filteredSearchQuery.IndexName;
                Fields = filteredSearchQuery.Fields;
                TermCondition = filteredSearchQuery.TermCondition;
                RangeCondition = filteredSearchQuery.RangeCondition;
                MatchCondition = filteredSearchQuery.MatchCondition;
            }

            public SortedSearchQuery<TPoco> ThenBy(ElasticSort sort)
            {
                SortingFields.Add(sort);
                return this;
            }

            /// <summary>
            /// Limits the size of the document result set.
            /// </summary>
            /// <param name="take"></param>
            /// <returns></returns>
            public LimitedSearchQuery<TPoco> Take(int take)
            {
                return new LimitedSearchQuery<TPoco>(this, take);
            }
        }

        public sealed class LimitedSearchQuery<TPoco> where TPoco : IElasticPoco
        {
            internal Type PocoType { get; }
            internal string IndexName { get; }
            internal List<ElasticField> Fields { get; } = new List<ElasticField>();
            internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
            internal ElasticTermCodition TermCondition { get; set; }
            internal ElasticMatchCodition MatchCondition { get; set; }
            internal ElasticRangeCondition RangeCondition { get; set; }
            internal int Size { get; set; } = 25;
            internal int From { get; set; } = 0;

            internal LimitedSearchQuery(SearchQuery<TPoco> searchQuery, int take)
            {
                PocoType = searchQuery.PocoType;
                IndexName = searchQuery.IndexName;
                Fields = searchQuery.Fields;
                Size = take;
            }

            internal LimitedSearchQuery(FilteredSearchQuery<TPoco> filteredSearchQuery, int take)
            {
                PocoType = filteredSearchQuery.PocoType;
                IndexName = filteredSearchQuery.IndexName;
                Fields = filteredSearchQuery.Fields;
                TermCondition = filteredSearchQuery.TermCondition;
                RangeCondition = filteredSearchQuery.RangeCondition;
                MatchCondition = filteredSearchQuery.MatchCondition;
                Size = take;
            }

            internal LimitedSearchQuery(SortedSearchQuery<TPoco> sortedSearchQuery, int take)
            {
                PocoType = sortedSearchQuery.PocoType;
                IndexName = sortedSearchQuery.IndexName;
                Fields = sortedSearchQuery.Fields;
                TermCondition = sortedSearchQuery.TermCondition;
                RangeCondition = sortedSearchQuery.RangeCondition;
                MatchCondition = sortedSearchQuery.MatchCondition;
                SortingFields = sortedSearchQuery.SortingFields;
                Size = take;
            }

            /// <summary>
            /// Skips a certain number of results.
            /// </summary>
            /// <param name="skip">Number of documents to skip (Offset).</param>
            /// <returns></returns>
            public LimitedSearchQuery<TPoco> Skip(int skip)
            {
                From = skip;
                return this;
            }
        }

    /*public abstract class Search : AbstractQuery
    {
        internal List<ElasticField> Fields { get; } = new List<ElasticField>();
        internal List<ElasticSort> SortingFields { get; } = new List<ElasticSort>();
        internal int Size { get; set; } = 25;
        internal int From { get; set; } = 0;

        protected Search(IElasticPoco poco) : base(poco) { }

        protected Search(string indexName, string typeName) : base(indexName, typeName) { }
    }

    public class Search<T> : Search, ISearchExecutable<T> where T : IElasticPoco
    {
        protected Search(string indexName, string typeName) : base(indexName, typeName) { }

        /// <summary>
        /// Prepares a Search request which will be executed on the given index and type.
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Search<T> In(string indexName, string typeName)
        {
            return new Search<T>(indexName, typeName);
        }
        /// <summary>
        /// Include certain fields to a query
        /// </summary>
        /// <param name="incluededFields"></param>
        /// <returns></returns>
        public Search<T> Include(params ElasticField[] incluededFields)
        {
            CheckParameters(incluededFields);
            var include = incluededFields.Where(inc => !Fields.Select(f => f.Name).Contains(inc.Name));
            Fields.AddRange(include);

            return this;
        }
        /// <summary>
        /// Exclude certain fields from a query
        /// </summary>
        /// <param name="excludeFields"></param>
        /// <returns></returns>
        public Search<T> Exclude(params ElasticField[] excludeFields)
        {
            CheckParameters(excludeFields);
            var exclude = excludeFields.Where(ex => !Fields.Select(f => f.Name).Contains(ex.Name));
            Fields.Clear();
            Fields.AddRange(exclude);

            return this;
        }
        /// <summary>
        /// Match Query
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="value">Value matching the field</param>
        /// <returns></returns>
        public ISearchExecutable<T> Match(string field, object value)
        {
            var condition = new ElasticCodition
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
        public ISearchExecutable<T> Match(ElasticCodition condition)
        {
            ClearAllConditions();
            MatchCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index.
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="value">Value which should equal the field content</param>
        /// <returns></returns>
        public ISearchExecutable<T> Term(string field, object value)
        {
            var condition = new ElasticCodition
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
        public ISearchExecutable<T> Term(ElasticCodition condition)
        {
            ClearAllConditions();
            TermCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Returns document for a range
        /// </summary>
        /// <param name="field">Field name</param>
        /// <param name="op">Range operator</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ISearchExecutable<T> Range(string field, ElasticRangeOperations op, object value)
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
        /// Returns document for a range
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ISearchExecutable<T> Range(ElasticRangeCondition condition)
        {
            ClearAllConditions();
            RangeCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Limits the size of the document result set.
        /// </summary>
        /// <param name="take"></param>
        /// <returns></returns>
        public ISearchExecutable<T> Take(int take)
        {
            Size = take;
            return this;
        }
        /// <summary>
        /// Skips a certain number of results.
        /// </summary>
        /// <param name="skip">Number of documents to skip (Offset).</param>
        /// <returns></returns>
        public ISearchExecutable<T> Skip(int skip)
        {
            From = skip;
            return this;
        }




        public Search<T> Sort(string field, ElasticSortOrders sortOrder)
        {
            return Sort(new ElasticField { Name = field }, sortOrder);
        }
        public Search<T> Sort(ElasticField field, ElasticSortOrders sortOrder)
        {
            SortingFields.Add(new ElasticSort { Field = field, Order = sortOrder });
            return this;
        }
    }*/
}
