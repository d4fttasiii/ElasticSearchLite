using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Models.Conditions
{
    public class ElasticMatchPhraseCondition : IElasticCondition
    {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
        public ElasticOperators Operation { get; set; }
        public int Slop { get; set; }
    }
}
