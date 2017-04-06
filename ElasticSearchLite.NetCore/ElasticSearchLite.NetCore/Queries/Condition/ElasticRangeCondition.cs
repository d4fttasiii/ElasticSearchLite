using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries.Condition
{
    public class ElasticRangeCondition
    {
        public IElasticField Field { get; set; }
        public RangeOperations Operation { get; set; }
        public string Value { get; set; }
    }
}
