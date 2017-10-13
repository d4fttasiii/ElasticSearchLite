using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticHighlight
    {
        public string PreTag { get; set; }
        public string PostTag { get; set; }
        protected internal int FragmentSize { get; set; }
        protected internal int NumberOfFragments { get; set; }
        public List<ElasticField> HighlightedFields { get; } = new List<ElasticField>();
    }
}
