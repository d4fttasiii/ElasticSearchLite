using ElasticSearchLite.NetCore.Interfaces;
using System;

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
            public MultiGetQuery<TPoco> ByIds<T>(params T[] ids)
                where T : class
            {
                Ids = ids;

                return this;
            }
        }
    }
}
