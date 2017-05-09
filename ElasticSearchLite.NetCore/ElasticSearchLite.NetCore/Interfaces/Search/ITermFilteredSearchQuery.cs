using System;
using System.Linq.Expressions;

namespace ElasticSearchLite.NetCore.Interfaces.Search
{
    public interface ITermFilteredSearchQuery<TPoco> : IFilteredSearchQuery<TPoco>
        where TPoco: IElasticPoco
    {
        /// <summary>
        /// Add another value to the term condition
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ITermFilteredSearchQuery<TPoco> Or(object value);
    }
}
