using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Models.Conditions
{
    public class ElasticMatchPhrasePrefixCondition : IElasticCondition {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
        public ElasticOperators Operation { get; set; }
        public int Fuzziness { get; set; }
    }
}
