using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticRangeCondition : IElasticCondition
    {
        public ElasticField Field { get; set; }
        public ElasticRangeOperations Operation { get; set; }
        public object Value { get; set; }
    }
}
