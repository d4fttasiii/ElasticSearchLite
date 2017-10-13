using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IBoolQueryHighlightingEnabled<TPoco>
        where TPoco: IElasticPoco
    {
        IBoolQueryHighlightingEnabled<TPoco> AddField(Expression<Func<TPoco, object>> propertyExpression);
        IBoolQueryHighlightingEnabled<TPoco> AddFields(IEnumerable<Expression<Func<TPoco, object>>> propertyExpressions);
        IBoolQueryPreAdded<TPoco> SetPreTagTo(string preTag);
    }
}
