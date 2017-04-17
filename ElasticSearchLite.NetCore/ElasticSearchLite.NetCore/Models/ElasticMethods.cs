namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMethods
    {
        public static ElasticMethods Index { get; } = new ElasticMethods("index");
        public static ElasticMethods Delete { get; } = new ElasticMethods("delete");
        public static ElasticMethods Update { get; } = new ElasticMethods("update");

        public string Name { get; }

        private ElasticMethods(string name)
        {
            Name = name;
        }
    }
}
