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
        internal List<ElasticCodition> Matches { get; }
        internal List<ElasticCodition> Terms { get; }

        protected SearchQuery()
        {
            Fields = new List<IElasticField>();
            Matches = new List<ElasticCodition>();
            Terms = new List<ElasticCodition>();
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

            return this;
        }

        public SearchQuery<T> Match(params ElasticCodition[] conditions)
        {
            CheckParameters(conditions);

            Matches.AddRange(conditions);

            return this;
        }

        public SearchQuery<T> Term(params ElasticCodition[] conditions)
        {
            CheckParameters(conditions);

            Terms.AddRange(conditions);

            return this;
        }

        private void CheckParameters<PT>(PT[] parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            if (!parameters.Any()) { throw new ArgumentException(nameof(parameters)); }
        }
    }
}
