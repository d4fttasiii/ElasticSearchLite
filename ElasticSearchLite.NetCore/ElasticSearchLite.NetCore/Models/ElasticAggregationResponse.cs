namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticAggregationResponse
    {
        public string KeyAsString { get; set; }
        public int? DocCount { get; set; }
        public double? AggregatedValue { get; set; }
        public double? MovingAverage { get; set; }
    }
}
