namespace ElasticSearchLite.NetCore.Models
{
    public sealed class ElasticBoolQueryOccurrences
    {
        public static ElasticBoolQueryOccurrences Must { get; } = new ElasticBoolQueryOccurrences("must");
        public static ElasticBoolQueryOccurrences MustNot { get; } = new ElasticBoolQueryOccurrences("must_not");
        public static ElasticBoolQueryOccurrences Should { get; } = new ElasticBoolQueryOccurrences("should");
        public static ElasticBoolQueryOccurrences Filter { get; } = new ElasticBoolQueryOccurrences("filter");

        public string Name { get; }

        private ElasticBoolQueryOccurrences(string name)
        {
            Name = name;
        }
    }
}
