namespace ElasticSearchLite.NetCore.Models
{
    public sealed class ElasticFields
    {
        public static ElasticFields Id { get; } = new ElasticFields("_id");
        public static ElasticFields Index { get; } = new ElasticFields("_index");
        public static ElasticFields Type { get; } = new ElasticFields("_type");
        public static ElasticFields Score { get; } = new ElasticFields("_score");
        public static ElasticFields Source { get; } = new ElasticFields("_source");
        public static ElasticFields Hits { get; } = new ElasticFields("hits");
        public static ElasticFields Total { get; } = new ElasticFields("total");
        public static ElasticFields Items { get; } = new ElasticFields("items");
        public static ElasticFields Indexed { get; } = new ElasticFields("indexed");
        public static ElasticFields Updated { get; } = new ElasticFields("updated");
        public static ElasticFields Deleted { get; } = new ElasticFields("deleted");

        public string Name { get; }

        private ElasticFields(string name)
        {
            Name = name;
        }
    }
}
