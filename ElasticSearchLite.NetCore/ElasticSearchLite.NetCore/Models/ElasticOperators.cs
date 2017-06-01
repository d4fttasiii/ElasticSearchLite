namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticOperators
    {
        public static ElasticOperators Or { get; } = new ElasticOperators("or");
        public static ElasticOperators And { get; } = new ElasticOperators("and");

        public string Name { get; set; }

        private ElasticOperators(string name)
        {
            Name = name;
        }
    }
}