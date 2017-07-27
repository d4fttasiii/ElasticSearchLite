using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models.Conditions
{
    public class ElasticMultiMatchCondition
    {
        public object Value { get; set; }
        public IEnumerable<ElasticField> Fields { get; set; }
    }
}
