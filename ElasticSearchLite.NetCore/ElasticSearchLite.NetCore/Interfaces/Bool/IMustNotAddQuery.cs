using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IMustNotAddQuery<TPoco> : IShouldAddQuery<TPoco> where TPoco : IElasticPoco { }
}
