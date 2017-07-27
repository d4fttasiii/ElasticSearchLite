namespace ElasticSearchLite.NetCore.Models.Enums
{
    public sealed class ElasticRangeOperations
    {
        public static ElasticRangeOperations Gt { get; } = new ElasticRangeOperations("gt");
        public static ElasticRangeOperations Lt { get; } = new ElasticRangeOperations("lt");
        public static ElasticRangeOperations Gte { get; } = new ElasticRangeOperations("gte");
        public static ElasticRangeOperations Lte { get; } = new ElasticRangeOperations("lte");

        public string Name { get; }

        private ElasticRangeOperations(string name)
        {
            Name = name;
        }
    }
}
