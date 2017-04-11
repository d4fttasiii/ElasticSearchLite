namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticRangeCondition
    {
        public ElasticField Field { get; set; }
        public RangeOperations Operation { get; set; }
        public object Value { get; set; }
    }
}
