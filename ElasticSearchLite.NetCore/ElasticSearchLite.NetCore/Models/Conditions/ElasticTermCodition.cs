using System.Collections.Generic;
using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Models.Conditions
{
    public class ElasticTermCodition : IElasticCondition
    {
        public ElasticField Field { get; set; }
        public List<object> Values { get; set; }
    }
}
