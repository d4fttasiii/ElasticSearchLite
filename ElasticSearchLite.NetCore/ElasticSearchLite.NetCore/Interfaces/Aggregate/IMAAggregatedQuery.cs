using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Aggregate
{
    public interface IMAAggregatedQuery<TPoco>
        where TPoco : IElasticPoco
    {
        /// <summary>
        /// Setting the date histogram for the moving average
        /// </summary>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        IMADateHistogramAddedAggregatedQuery<TPoco> SetDateHistogramField(Expression<Func<TPoco, object>> propertyExpression);
    }
}
