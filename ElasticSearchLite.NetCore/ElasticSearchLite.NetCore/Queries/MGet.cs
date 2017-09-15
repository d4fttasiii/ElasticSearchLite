using ElasticSearchLite.NetCore.Interfaces;
using System;
using ElasticSearchLite.NetCore.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Queries
{
    public class MGet
    {
        private string IndexName { get; }

        protected MGet(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentException($"The given index name cannot be empty!"); }

            IndexName = indexName;
        }
        /// <summary>
        /// Creates aa MGet API request which can return a multiple documents based on id.
        /// </summary>
        /// <param name="indexName">Name or alias of the index</param>
        /// <returns></returns>
        public static MGet FromIndex(string indexName) => new MGet(indexName);
        /// <summary>
        /// Sets the return type
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public MultiGetQuery<TPoco> Returns<TPoco>() where TPoco : IElasticPoco => new MultiGetQuery<TPoco>(IndexName);

        public abstract class MultiGetQuery : AbstractBaseQuery
        {
            protected internal object[] Ids { get; set; }
            protected internal List<ElasticField> SourceFields = new List<ElasticField>();

            protected MultiGetQuery(string indexName) : base(indexName) { }
        }

        public sealed class MultiGetQuery<TPoco> : MultiGetQuery where TPoco : IElasticPoco
        {
            internal MultiGetQuery(string indexName) : base(indexName) { }
            /// <summary>
            /// Set request ids
            /// </summary>
            /// <param name="ids">Document Ids</param>
            /// <returns></returns>
            public MultiGetQuery<TPoco> ByIds(params object[] ids)
            {
                Ids = ids;

                return this;
            }
            /// <summary>
            /// Allows to control how the _source field is returned with every hit.
            /// Every field will be returned by default.
            /// </summary>
            /// <param name="propertyExpression"></param>
            /// <returns></returns>
            public MultiGetQuery<TPoco> SelectField(Expression<Func<TPoco, object>> propertyExpression)
            {
                SourceFields.Add(new ElasticField
                {
                    UseKeywordField = false,
                    Name = GetCorrectPropertyName(propertyExpression)
                });

                return this;
            }
        }
    }
}
