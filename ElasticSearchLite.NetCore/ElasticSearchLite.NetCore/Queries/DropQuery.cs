using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class DropQuery : AbstractQuery
    {
        protected DropQuery(IElasticPoco poco) : base(poco) { }

        protected DropQuery(string indexName, string typeName) : base(indexName, typeName) { }

        protected override void ClearAllConditions()
        {
        }
    }

    public class DropQuery<T> : DropQuery where T : IElasticPoco
    {
        protected DropQuery(T poco) : base(poco) { }

        protected DropQuery(string indexName) : base(indexName, "") { }

        public static DropQuery<T> Create(T poco)
        {
            return new DropQuery<T>(poco);
        }

        public static DropQuery<T> Create(string indexName)
        {
            return new DropQuery<T>(indexName);
        }
    }
}
