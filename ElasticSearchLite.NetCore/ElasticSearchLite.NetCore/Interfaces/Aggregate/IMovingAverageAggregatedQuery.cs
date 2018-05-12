namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IMovingAverageAggregatedQuery<TPoco>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// The size of window to "slide" across the histogram.
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        IExecutableAggregatedQuery<TPoco> SetWindow(int windowSize);
    }
}
