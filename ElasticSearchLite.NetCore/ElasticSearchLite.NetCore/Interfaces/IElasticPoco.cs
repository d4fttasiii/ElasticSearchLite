namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IElasticPoco
    {
        /// <summary>
        /// The document’s ID.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/mapping-id-field.html
        /// </summary>
        string Id { get; set; }
        /// <summary>
        /// The document’s mapping type.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/mapping-type-field.html
        /// </summary>
        string Type { get; set; }
        /// <summary>
        /// The index to which the document belongs.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/mapping-index-field.html
        /// </summary>
        string Index { get; set; }
        /// <summary>
        /// A value which describes the document's relevance for a certain search.
        /// </summary>
        double? Score { get; set; }
        /// <summary>
        /// Number of total documents found
        /// </summary>
        long Total { get; set; }
        /// <summary>
        /// Each indexed document is given a version number. 
        /// The associated version number is returned as part of the response to the index API request. 
        /// The index API optionally allows for optimistic concurrency control when the version parameter is specified.
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.5/docs-index_.html#index-versioning
        /// </summary>
        int? Version { get; set; }
    }
}
