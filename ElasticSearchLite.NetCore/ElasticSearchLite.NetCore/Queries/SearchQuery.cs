using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchLite.NetCore.Queries
{
    public class SearchQuery : IQuery
    {
        internal string IndexName { get; set; }
        internal bool IsMatchAll { get; set; }
        internal List<IElasticField> Fields { get; }
        internal ElasticCodition MatchCondition { get; set; }
        internal List<ElasticCodition> MatchConditions { get; }
        internal ElasticCodition TermCondition { get; set; }

        protected SearchQuery()
        {
            Fields = new List<IElasticField>();
            MatchConditions = new List<ElasticCodition>();
        }
    }

    public class SearchQuery<T> : SearchQuery where T : IElasticPoco
    {
        public SearchQuery() : base() { }

        public SearchQuery<T> Index(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentException(nameof(indexName)); }

            IndexName = indexName;

            return this;
        }

        public SearchQuery<T> Include(params IElasticField[] incluededFields)
        {
            CheckParameters(incluededFields);
            var include = incluededFields.Where(inc => !Fields.Select(f => f.Name).Contains(inc.Name));
            Fields.AddRange(include);

            return this;
        }

        public SearchQuery<T> Exclude(params IElasticField[] excludeFields)
        {
            CheckParameters(excludeFields);
            var exclude = excludeFields.Where(ex => !Fields.Select(f => f.Name).Contains(ex.Name));
            Fields.Clear();
            Fields.AddRange(exclude);

            return this;
        }

        public SearchQuery<T> MatchAll(bool matchAll)
        {
            IsMatchAll = matchAll;
            ClearAllFilters();

            return this;
        }
        /// <summary>
        /// Match Query
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SearchQuery<T> Match(ElasticCodition condition)
        {
            CheckParameter(condition);
            ClearAllFilters();
            MatchCondition = condition;

            return this;
        }

        public SearchQuery<T> MultiMatch(params ElasticCodition[] conditions)
        {
            CheckParameters(conditions);
            ClearAllFilters();
            MatchConditions.AddRange(conditions);

            return this;
        }
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SearchQuery<T> Term(ElasticCodition condition)
        {
            CheckParameter(condition);
            ClearAllFilters();
            TermCondition = condition;

            return this;
        }

        private void CheckParameters<PT>(PT[] parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            if (!parameters.Any()) { throw new ArgumentException(nameof(parameters)); }
        }

        private void CheckParameter<PT>(PT parameter)
        {
            if (parameter == null) { throw new ArgumentNullException(nameof(parameter)); }
        }

        private void ClearAllFilters()
        {
            TermCondition = null;
            MatchCondition = null;
            MatchConditions.Clear();
        }
    }
}
