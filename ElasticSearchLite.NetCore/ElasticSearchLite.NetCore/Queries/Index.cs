using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Index : AbstractQuery
    {
        protected Index(IElasticPoco poco) : base(poco) { }

        protected override void ClearAllConditions()
        {
            MatchCondition = null;
            RangeCondition = null;
            TermCondition = null;
        }
    }

    public class Index<T> : Index where T : IElasticPoco
    {
        protected Index(T poco) : base(poco) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poco"></param>
        /// <returns></returns>
        public static Index<T> Document(T poco)
        {
            return new Index<T>(poco);
        }
    }
}
