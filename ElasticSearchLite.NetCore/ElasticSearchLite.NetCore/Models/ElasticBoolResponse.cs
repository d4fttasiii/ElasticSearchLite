using ElasticSearchLite.NetCore.Interfaces;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticBoolResponse<TPoco>
        where TPoco : IElasticPoco
    {
        public TPoco Poco { get; set; }
        public Dictionary<string, string[]> Highlight { get; set; }
    }
}
