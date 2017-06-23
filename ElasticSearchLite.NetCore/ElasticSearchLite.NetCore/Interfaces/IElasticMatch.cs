using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces
{
    public interface IElasticMatch
    {
        ElasticField Field { get; set; }
        object Value { get; set; }
    }
}
