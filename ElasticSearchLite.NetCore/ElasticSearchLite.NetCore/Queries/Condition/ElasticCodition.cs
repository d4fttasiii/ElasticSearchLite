using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries.Condition
{
    public class ElasticCodition
    {
        public IElasticField Field { get; set; }
        public string Value { get; set; }
    }
}
