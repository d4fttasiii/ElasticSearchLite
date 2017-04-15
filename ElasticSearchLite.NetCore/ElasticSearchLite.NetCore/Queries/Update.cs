using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Update : AbstractQuery
    {
        protected Update(IElasticPoco poco) : base(poco) { }

        protected override void ClearAllConditions()
        {
            MatchCondition = null;
            RangeCondition = null;
            TermCondition = null;
        }
    }

    public class Update<T> : Update where T : IElasticPoco
    {
        protected Update(IElasticPoco poco) : base(poco) { }

        public static Update<T> Document(T poco)
        {
            return new Update<T>(poco);
        }
    }
}
