using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;

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

        public static DeleteQuery<T> Create(T poco)
        {
            return new DeleteQuery<T>(poco);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DeleteQuery<T> Match(string field, object value)
        {
            var condition = new ElasticCodition
            {
                Field = new ElasticField { Name = field },
                Value = value
            };

            return Match(condition);
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
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DeleteQuery<T> Term(string field, object value)
        {
            var condition = new ElasticCodition
            {
                Field = new ElasticField { Name = field },
                Value = value
            };

            return Term(condition);
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
        public DeleteQuery<T> Range(string field, RangeOperations operation, object value)
        {
            var rangeCondition = new ElasticRangeCondition
            {
                Field = new ElasticField { Name = field },
                Operation = operation,
                Value = value
            };

            return Range(rangeCondition);
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
