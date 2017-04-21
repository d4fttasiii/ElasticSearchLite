using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IExecutableSearch<TPoco> : IQuery where TPoco : IElasticPoco
    {
    }
}
