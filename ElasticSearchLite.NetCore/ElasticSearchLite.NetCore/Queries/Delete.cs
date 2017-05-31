using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Delete
    {
        private string IndexName { get; }

        private Delete(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentNullException(nameof(IndexName)); }

            IndexName = indexName;
        }
        /// <summary>
        /// Creates a DeleteQuery instance which can be specified with Term, Match, Range methods.
        /// This DeleteQuery will be executed using the Delete by Query API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete-by-query.html
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static Delete From(string indexName) => new Delete(indexName);
        /// <summary>
        /// This DeleteQuery will be executed using the Delete API and remove one document where the 
        /// Id is the given poco's Id.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-delete.html
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="poco"></param>
        /// <returns></returns>
        public static IDeleteExecutable<TPoco> Document<TPoco>(TPoco poco)
            where TPoco : IElasticPoco => new DeleteQuery<TPoco>(poco);
        /// <summary>
        /// Sets the poco type used in the query builiding process.
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public DeleteQuery<TPoco> Documents<TPoco>()
            where TPoco : IElasticPoco => new DeleteQuery<TPoco>(IndexName);

        public abstract class DeleteQuery : AbstractConditionalQuery
        {
            internal DeleteQuery(IElasticPoco poco) : base(poco)
            {
                if (string.IsNullOrEmpty(poco.Id))
                {
                    throw new ArgumentNullException(nameof(poco.Id));
                }
            }
            internal DeleteQuery(string indexName) : base(indexName) { }
        }

        public sealed class DeleteQuery<TPoco> : DeleteQuery, IDeleteExecutable<TPoco>
        where TPoco : IElasticPoco
        {
            internal DeleteQuery(IElasticPoco poco) : base(poco) { }
            internal DeleteQuery(string indexName) : base(indexName) { }
            /// <summary>
            /// Creates a DeleteQuery instance which will delete a certain document associated with the given POCO.
            /// This DeleteQuery will be executed using the Delete API
            /// https://www.elastic.co/guide/en/elasticsearch/reference/current/docs-delete.html
            /// </summary>
            /// <param name="poco">A valid Poco should have and Index, Type and Id</param>
            /// <returns>Return an executeable DeleteQuery</returns>
            public IDeleteExecutable<TPoco> Document(TPoco poco) => new DeleteQuery<TPoco>(poco);
            /// <summary>
            /// Deletes a document for a match condition
            /// </summary>
            /// <param name="condition"></param>
            /// <returns>Returns the updated DeleteQuery object.</returns>
            public IDeleteExecutable<TPoco> Match(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                var condition = new ElasticMatchCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };
                MatchCondition = condition;

                return this;
            }
            /// <summary>
            /// Deletes document for a term condition
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="value"></param>
            /// <returns>Returns the updated DeleteQuery object.</returns>
            public IDeleteExecutable<TPoco> Term(Expression<Func<TPoco, object>> propertyExpression, object value)
            {
                var condition = new ElasticTermCodition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };
                TermConditions.Add(condition);

                return this;
            }
            /// <summary>
            /// Deletes documents which are in a certain range
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <param name="operation"></param>
            /// <param name="value"></param>
            /// <returns>Returns the updated DeleteQuery object.</returns>
            public IDeleteExecutable<TPoco> Range(Expression<Func<TPoco, object>> propertyExpression, ElasticRangeOperations operation, object value)
            {
                var rangeCondition = new ElasticRangeCondition
                {
                    Field = new ElasticField { Name = GetCorrectPropertyName(propertyExpression) },
                    Operation = operation ?? throw new ArgumentNullException(nameof(operation)),
                    Value = value ?? throw new ArgumentNullException(nameof(value))
                };
                RangeConditions.Add(rangeCondition);

                return this;
            }
        }

    }

}
