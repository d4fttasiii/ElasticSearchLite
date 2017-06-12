using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticTermCodition
    {
        public ElasticField Field { get; set; }
        public List<object> Values { get; set; }
    }
}
