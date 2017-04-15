using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Search : AbstractQuery
    {
        internal List<ElasticField> Fields { get; } = new List<ElasticField>();

        protected Search(IElasticPoco poco) : base(poco) { }

        protected Search(string indexName, string typeName) : base(indexName, typeName) { }

        protected override void ClearAllConditions()
        {
            TermCondition = null;
            RangeCondition = null;
            MatchCondition = null;
        }
    }
    public class Search<T> : Search where T : IElasticPoco
    {
        protected Search(string indexName, string typeName) : base(indexName, typeName) { }

        /// <summary>
        /// 
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
        public Search<T> Match(string field, object value)
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
        public Search<T> Match(ElasticCodition condition)
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
        public Search<T> Term(string field, object value)
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
        public Search<T> Term(ElasticCodition condition)
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
        public Search<T> Range(string field, ElasticRangeOperations op, object value)
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
        public Search<T> Range(ElasticRangeCondition condition)
        {
            ClearAllConditions();
            RangeCondition = CheckParameter(condition);

            return this;
        }
    }
}
