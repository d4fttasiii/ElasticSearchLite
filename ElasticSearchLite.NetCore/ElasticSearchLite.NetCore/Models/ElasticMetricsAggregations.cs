namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMetricsAggregations
    {
        public string Name { get; }

        private ElasticMetricsAggregations(string name)
        {
            Name = name;
        }

        public static ElasticMetricsAggregations Avg => new ElasticMetricsAggregations("avg");
        public static ElasticMetricsAggregations Cardinality => new ElasticMetricsAggregations("cardinality");
        public static ElasticMetricsAggregations Max => new ElasticMetricsAggregations("max");
        public static ElasticMetricsAggregations Min => new ElasticMetricsAggregations("min");
        public static ElasticMetricsAggregations Sum => new ElasticMetricsAggregations("sum");
        public static ElasticMetricsAggregations ValueCount => new ElasticMetricsAggregations("value_count");
    }
}
