using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMapping
    {
        public string Name { get; set; }
        public ElasticCoreFieldDataTypes FieldDataType { get; set; }
        public ElasticAnalyzers Analyzer { get; set; }
    }
}
