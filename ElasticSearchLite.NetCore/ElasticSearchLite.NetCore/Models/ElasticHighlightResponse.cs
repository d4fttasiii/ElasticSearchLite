using ElasticSearchLite.NetCore.Interfaces;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticHighlightResponse : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }
        public long Total { get; set; }
        public Dictionary<string, string[]> Highlight { get; set; } = new Dictionary<string, string[]>();
        public int? Version { get; set; }
    }
}
