namespace ElasticSearchLite.NetCore.Queries.Condition
{
    public sealed class RangeOperations
    {
        public static readonly RangeOperations Gt = new RangeOperations("gt");
        public static readonly RangeOperations Lt = new RangeOperations("lt");
        public static readonly RangeOperations Gte = new RangeOperations("gte");
        public static readonly RangeOperations Lte = new RangeOperations("lte");

        public readonly string Name;

        private RangeOperations(string name)
        {
            Name = name;
        }
    }
}
