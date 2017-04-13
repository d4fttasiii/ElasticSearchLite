using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class IndexQuery : AbstractQuery
    {
        protected IndexQuery(IElasticPoco poco) : base(poco) { }

        protected override void ClearAllConditions()
        {
            MatchCondition = null;
            RangeCondition = null;
            TermCondition = null;
        }
    }

    public class IndexQuery<T> : IndexQuery where T : IElasticPoco
    {
        protected IndexQuery(T poco) : base(poco) { }

        public static IndexQuery<T> Create(T poco)
        {
            return new IndexQuery<T>(poco);
        }
    }
}
