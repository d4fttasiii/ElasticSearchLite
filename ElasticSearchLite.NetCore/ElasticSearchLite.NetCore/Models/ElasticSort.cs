using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticSort
    {
        public ElasticField Field { get; set; }
        public ElasticSortOrders Order { get; set; }
    }
}
