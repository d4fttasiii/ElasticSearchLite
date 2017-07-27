namespace ElasticSearchLite.NetCore.Models.Enums
{
    public class ElasticSortOrders
    {
        public static ElasticSortOrders Descending { get; } = new ElasticSortOrders("desc");
        public static ElasticSortOrders Ascending { get; } = new ElasticSortOrders("asc");

        public string Name { get; }

        private ElasticSortOrders(string name)
        {
            Name = name;
        }
    }
}
