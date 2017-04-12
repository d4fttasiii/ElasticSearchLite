using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class InsertQuery : AbstractQuery
    {
        protected InsertQuery(IElasticPoco poco) : base(poco) { }

        protected override void ClearAllConditions()
        {
            MatchCondition = null;
            RangeCondition = null;
            TermCondition = null;
        }
    }

    public class InsertQuery<T> : InsertQuery where T : IElasticPoco
    {
        protected InsertQuery(T poco) : base(poco) { }

        public static InsertQuery<T> Create(T poco)
        {
            return new InsertQuery<T>(poco);
        }
    }
}
