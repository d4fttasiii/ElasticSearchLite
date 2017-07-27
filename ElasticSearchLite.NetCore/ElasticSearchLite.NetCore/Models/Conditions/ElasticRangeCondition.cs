using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Models.Conditions
{
    public class ElasticRangeCondition : IElasticCondition
    {
        public ElasticField Field { get; set; }
        public ElasticRangeOperations Operation { get; set; }
        public object Value { get; set; }
    }
}
