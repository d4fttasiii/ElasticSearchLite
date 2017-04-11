using ElasticSearchLite.NetCore.Queries.Models;

namespace ElasticSearchLite.NetCore.Queries.Condition
{
    public class ElasticRangeCondition
    {
        public ElasticField Field { get; set; }
        public RangeOperations Operation { get; set; }
        public object Value { get; set; }
    }
}
