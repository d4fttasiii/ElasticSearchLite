namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IMADateHistogramAddedAggregatedQuery<TPoco>
        where TPoco: IElasticPoco
    {
        /// <summary>
        /// Setting interval.
        /// Examples: 1m, 1h, 1d, 1M, 1y
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        IMAIntervalSetAggregatedQuery<TPoco> SetDateHistogramInterval(string interval);
    }
}
