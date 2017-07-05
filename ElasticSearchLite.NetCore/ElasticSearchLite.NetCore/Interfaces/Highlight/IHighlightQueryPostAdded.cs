using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryPostAdded<TPoco> where TPoco: IElasticPoco
    {
        IHighlightQueryExecutable<TPoco> AddFields(params Expression<Func<TPoco, object>>[] propertyExpressions);
    }
}
