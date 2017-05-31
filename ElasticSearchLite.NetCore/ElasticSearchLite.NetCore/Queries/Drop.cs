using ElasticSearchLite.NetCore.Interfaces;
using System;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Drop : IQuery
    {
        public string IndexName { get; }

        protected Drop(string indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
        }

        /// <summary>
        /// Creates a new DropQuery instance which will remove a certain index from the ElasticSearch
        /// using the Delete Index API
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/indices-delete-index.html
        /// </summary>
        /// <param name="indexName">Index's which should be removed.</param>
        /// <returns>DropQuery object.</returns>
        public static Drop Index(string indexName) => new Drop(indexName);
    }
}
