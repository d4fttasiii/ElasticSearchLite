using ElasticSearchLite.NetCore.Interfaces.Bool;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryExecutable<TPoco> : IQuery where TPoco : IElasticPoco
    {
        IHighlightQueryShouldAdded<TPoco> Should(Expression<Func<TPoco, object>> propertyExpression);
        IHighlightQueryMustAdded<TPoco> Must(Expression<Func<TPoco, object>> propertyExpression);
        IHighlightQueryMustNotAdded<TPoco> MustNot(Expression<Func<TPoco, object>> propertyExpression);
        IHighlightQueryFilterAdded<TPoco> Filter(Expression<Func<TPoco, object>> propertyExpression);
        IHighlightQueryExecutable<TPoco> Take(int take);
        IHighlightQueryExecutable<TPoco> Skip(int skip);
    }
}
