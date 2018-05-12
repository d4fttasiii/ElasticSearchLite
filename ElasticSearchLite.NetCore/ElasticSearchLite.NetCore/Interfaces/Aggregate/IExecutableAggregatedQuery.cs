namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IExecutableAggregatedQuery<TPoco> : IQuery
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// The default value of alpha is 0.3, and the setting accepts any float from 0-1 inclusive.
        /// </summary>
        /// <param name="alpha"></param>
        /// <returns></returns>
        IExecutableAggregatedQuery<TPoco> SetAlpha(float alpha);
    }
}
