namespace ElasticSearchLite.NetCore.Queries
{
    public class Get : AbstractBaseQuery
    {
        internal object Id { get; set; }

        protected Get(string indexName) : base(indexName) { }
        /// <summary>
        /// Creates a Get API request which can return a document by id.
        /// </summary>
        /// <param name="indexName">Name or alias of the index</param>
        /// <returns></returns>
        public static Get FromIndex(string indexName) => new Get(indexName);
        /// <summary>
        /// Set request id
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns></returns>
        public Get ById(object id)
        {
            Id = id;

            return this;
        }
    }
}
