using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Delete : AbstractQuery, IDeleteExecutable
    {
        protected Delete(IElasticPoco poco) : base(poco)
        {
            if (string.IsNullOrEmpty(poco.Id))
            {
                throw new ArgumentNullException(nameof(poco.Id));
            }
        }
        protected Delete(string indexName, string typeName) : base(indexName, typeName) { }
        /// <summary>
        /// Creates a DeleteQuery instance which can be specified with Term, Match, Range methods.
        /// This DeleteQuery will be executed using the Delete by Query API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Delete From(string indexName, string typeName)
        {
            return new Delete(indexName, typeName);
        }
        /// <summary>
        /// Creates a DeleteQuery instance which will delete a certain document associated with the given POCO.
        /// This DeleteQuery will be executed using the Delete API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete.html
        /// </summary>
        /// <param name="poco">A valid Poco should have and Index, Type and Id</param>
        /// <returns>Return an executeable DeleteQuery</returns>
        public static IDeleteExecutable Document<TPoco>(TPoco poco) where TPoco : IElasticPoco
        {
            return new Delete(poco);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Match(string field, object value)
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
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Match(ElasticCodition condition)
        {
            MatchCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Term(string field, object value)
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
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Term(ElasticCodition condition)
        {
            TermCondition = CheckParameter(condition);

            return this;
        }
        /// <summary>
        /// Deletes documents which are in a certain range
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Range(string field, ElasticRangeOperations operation, object value)
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
        /// <returns>Returns the updated DeleteQuery object.</returns>
        public IDeleteExecutable Range(ElasticRangeCondition condition)
        {
            RangeCondition = CheckParameter(condition);

            return this;
        }
    }
}
