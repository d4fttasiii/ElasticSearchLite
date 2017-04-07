using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class UpdateQuery : AbstractQuery
    {
        protected UpdateQuery(IElasticPoco poco) : base(poco) { }

        protected override void ClearAllConditions()
        {
            MatchCondition = null;
            RangeCondition = null;
            TermCondition = null;
        }
    }

    public class UpdateQuery<T> : UpdateQuery where T : IElasticPoco
    {
        protected UpdateQuery(IElasticPoco poco) : base(poco) { }

        public UpdateQuery<T> Create(T poco)
        {
            return new UpdateQuery<T>(poco);
        }
    }
}
