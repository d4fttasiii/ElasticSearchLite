namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticPipelineAggregations
    {
        public string Name { get; }

        private ElasticPipelineAggregations(string name)
        {
            Name = name;
        }

        public static ElasticPipelineAggregations SimpleMovingAverage => new ElasticPipelineAggregations("simple");
        public static ElasticPipelineAggregations LinearMovingAverage => new ElasticPipelineAggregations("linear");
        public static ElasticPipelineAggregations ExponentiallyWeightedMovingAverage => new ElasticPipelineAggregations("ewma");
    }
}
