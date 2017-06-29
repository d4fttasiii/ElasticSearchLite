using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces.Highlight
{
    public interface IHighlightQueryUnconfigured<TPoco> where TPoco : IElasticPoco
    {
        IHighlightQueryPreAdded<TPoco> WithPre(string preTag);
    }
}
