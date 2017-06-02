using ElasticSearchLite.NetCore.Interfaces;
using System;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Get
    {
        private string IndexName { get; }

        protected Get(string indexName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentException($"The given index name cannot be empty!"); }

            IndexName = indexName;
        }
        /// <summary>
        /// Creates a Get API request which can return a document by id.
        /// </summary>
        /// <param name="indexName">Name or alias of the index</param>
        /// <returns></returns>
        public static Get FromIndex(string indexName) => new Get(indexName);
        /// <summary>
        /// Sets the return Poco type
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <returns></returns>
        public GetQuery<TPoco> Return<TPoco>() where TPoco : IElasticPoco => new GetQuery<TPoco>(IndexName);       

        public sealed class GetQuery<TPoco> : AbstractBaseQuery where TPoco : IElasticPoco
        {
            internal GetQuery(string indexName) : base(indexName) { }

            internal object Id { get; set; }
            /// <summary>
            /// Set request id
            /// </summary>
            /// <param name="id">Document Id</param>
            /// <returns></returns>
            public GetQuery<TPoco> ById(object id)
            {
                Id = id;

                return this;
            }
        }
    }
}
