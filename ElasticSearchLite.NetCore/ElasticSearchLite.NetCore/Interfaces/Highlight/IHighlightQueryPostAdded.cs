using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryPostAdded<TPoco> where TPoco: IElasticPoco
    {
        IHighlightQueryExecutable<TPoco> AddFields(params Expression<Func<TPoco, object>>[] propertyExpressions);
    }
}
