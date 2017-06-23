using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IExecutableBoolQuery<TPoco> where TPoco : IElasticPoco
    {
        IShouldAddQuery<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression);
        IMustAddQuery<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression);
        IMustNotAddQuery<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression);
        IFilterAddQuery<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression);
    }
}
