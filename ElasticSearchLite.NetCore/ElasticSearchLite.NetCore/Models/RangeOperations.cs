namespace ElasticSearchLite.NetCore.Models
{
    public sealed class RangeOperations
    {
        public static RangeOperations Gt { get; } = new RangeOperations("gt");
        public static RangeOperations Lt { get; } = new RangeOperations("lt");
        public static RangeOperations Gte { get; } = new RangeOperations("gte");
        public static RangeOperations Lte { get; } = new RangeOperations("lte");

        public string Name { get; }

        private RangeOperations(string name)
        {
            Name = name;
        }
    }
}
