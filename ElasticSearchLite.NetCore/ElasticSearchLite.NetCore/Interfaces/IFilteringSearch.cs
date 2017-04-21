using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IFilteringSearch<TPoco> : IExecutableSearch<TPoco> where TPoco : IElasticPoco
    {
        IExecutableSearch<TPoco> Limit(long limit);
    }
}
