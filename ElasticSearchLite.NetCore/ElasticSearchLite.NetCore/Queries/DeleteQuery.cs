using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Condition;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class DeleteQuery : AbstractQuery
    {
        protected DeleteQuery(IElasticPoco poco) : base(poco) { }

        protected DeleteQuery(string indexName, string typeName) : base(indexName, typeName) { }

        protected override void ClearAllConditions()
        {
            TermCondition = null;
            MatchCondition = null;
            RangeCondition = null;
        }
    }
    public class DeleteQuery<T> : DeleteQuery where T : IElasticPoco
    {
        protected DeleteQuery(IElasticPoco poco) : base(poco) { }

        protected DeleteQuery(string indexName, string typeName) : base(indexName, typeName) { }

        public static DeleteQuery<T> Create(string indexName, string typeName)
        {
            return new DeleteQuery<T>(indexName, typeName);
        }

        public static DeleteQuery<T> Create(IElasticPoco poco)
        {
            return new DeleteQuery<T>(poco);
        }

        /// <summary>
        /// If set to true it matches all documents
        /// </summary>
        /// <param name="matchAll"></param>
        /// <returns></returns>
        public DeleteQuery<T> MatchAll(bool matchAll)
        {
            IsMatchAll = matchAll;
            ClearAllConditions();

            return this;
        }
        /// <summary>
        /// Deletes documents matching a given condition
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DeleteQuery<T> Match(ElasticCodition condition)
        {
            ClearAllConditions();
            MatchCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Term Query finds documents that contain the exact term specified in the inverted index and deletes them.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DeleteQuery<T> Term(ElasticCodition condition)
        {            
            ClearAllConditions();
            TermCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Deletes documents which are in a certain range
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DeleteQuery<T> Range(ElasticRangeCondition condition)
        {
            ClearAllConditions();
            RangeCondition = CheckParameter(condition);

            return this;
        }
    }
}
