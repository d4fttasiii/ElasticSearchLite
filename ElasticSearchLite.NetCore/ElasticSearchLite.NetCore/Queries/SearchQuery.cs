using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchLite.NetCore.Queries
{
    public class SearchQuery : AbstractQuery
    {
        internal List<IElasticField> Fields { get; } = new List<IElasticField>();

        protected SearchQuery(IElasticPoco poco) : base(poco) { }

        protected SearchQuery(string indexName, string typeName) : base(indexName, typeName) { }

        protected override void ClearAllConditions()
        {
            TermCondition = null;
            RangeCondition = null;
            MatchCondition = null;
        }
    }
    public class SearchQuery<T> : SearchQuery where T : IElasticPoco
    {
        public SearchQuery(IElasticPoco poco) : base(poco) { }

        public SearchQuery(string indexName, string typeName) : base(indexName, typeName) { }

        /// <summary>
        /// Include certain fields to a query
        /// </summary>
        /// <param name="incluededFields"></param>
        /// <returns></returns>
        public SearchQuery<T> Include(params IElasticField[] incluededFields)
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
        public SearchQuery<T> Exclude(params IElasticField[] excludeFields)
        {
            CheckParameters(excludeFields);
            var exclude = excludeFields.Where(ex => !Fields.Select(f => f.Name).Contains(ex.Name));
            Fields.Clear();
            Fields.AddRange(exclude);

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchAll"></param>
        /// <returns></returns>
        public SearchQuery<T> MatchAll(bool matchAll)
        {
            IsMatchAll = matchAll;
            ClearAllConditions();

            return this;
        }
        /// <summary>
        /// Match Query
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SearchQuery<T> Match(ElasticCodition condition)
        {
            ClearAllConditions();
            MatchCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SearchQuery<T> Term(ElasticCodition condition)
        {
            ClearAllConditions();
            TermCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Returns document for a range
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public SearchQuery<T> Range(ElasticRangeCondition condition)
        {
            ClearAllConditions();
            RangeCondition = CheckParameter(condition);

            return this;
        }
    }
}
