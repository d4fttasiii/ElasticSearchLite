using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticHighlight
    {
        public string PreTag { get; set; }
        public string PostTag { get; set; }
        public List<ElasticField> HighlightedFields { get; } = new List<ElasticField>();
    }
}
