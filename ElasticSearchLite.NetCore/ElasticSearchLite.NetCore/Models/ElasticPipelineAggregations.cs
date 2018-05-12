namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticPipelineAggregations
    {
        public string Name { get; }

        private ElasticPipelineAggregations(string name)
        {
            Name = name;
        }

        public static ElasticPipelineAggregations SimpleMovingAverage => new ElasticPipelineAggregations("moving_avg");
        public static ElasticPipelineAggregations LinearMovingAverage => new ElasticPipelineAggregations("moving_avg");
        public static ElasticPipelineAggregations ExponentiallyWeightedMovingAverage => new ElasticPipelineAggregations("moving_avg");
    }
}
