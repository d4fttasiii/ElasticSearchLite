using System;
using ElasticSearchLite.NetCore.Queries.Models;

namespace ElasticSearchLite.NetCore.Queries.Condition
{
    public class ElasticCodition
    {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
    }
}
