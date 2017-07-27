using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticSort
    {
        public ElasticField Field { get; set; }
        public ElasticSortOrders Order { get; set; }
    }
}
