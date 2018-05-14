namespace ElasticSearchLite.NetCore.Models.Enums
{
    public sealed class ElasticFields
    {
        public static ElasticFields Id { get; } = new ElasticFields("_id");
        public static ElasticFields Index { get; } = new ElasticFields("_index");
        public static ElasticFields Type { get; } = new ElasticFields("_type");
        public static ElasticFields Score { get; } = new ElasticFields("_score");
        public static ElasticFields Version { get; } = new ElasticFields("_version");
        public static ElasticFields Source { get; } = new ElasticFields("_source");
        public static ElasticFields Shards { get; } = new ElasticFields("_shards");
        public static ElasticFields Docs { get; } = new ElasticFields("docs");
        public static ElasticFields Found { get; } = new ElasticFields("found");
        public static ElasticFields Hits { get; } = new ElasticFields("hits");
        public static ElasticFields Highlight { get; } = new ElasticFields("highlight");
        public static ElasticFields Total { get; } = new ElasticFields("total");
        public static ElasticFields Items { get; } = new ElasticFields("items");
        public static ElasticFields Successful { get; } = new ElasticFields("successful");
        public static ElasticFields Aggregations { get; } = new ElasticFields("aggregations");
        public static ElasticFields Value { get; } = new ElasticFields("value");
        public static ElasticFields MyDateHistogram { get; } = new ElasticFields("my_date_histo");
        public static ElasticFields Buckets { get; } = new ElasticFields("buckets");
        public static ElasticFields MovingAverage { get; } = new ElasticFields("the_movavg");

        public string Name { get; }

        private ElasticFields(string name)
        {
            Name = name;
        }
    }
}
