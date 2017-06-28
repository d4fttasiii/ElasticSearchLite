using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryExecutable<TPoco> : IQuery where TPoco : IElasticPoco
    {
        IBoolQueryShouldAdded<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression);
        IBoolQueryMustAdded<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression);
        IBoolQueryMustNotAdded<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression);
        IBoolQueryFilterAdded<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression);
        IBoolQueryExecutable<TPoco> Take(int take);
        IBoolQueryExecutable<TPoco> Skip(int skip);
    }
}
