using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Update : AbstractQuery
    {
        protected Update(IElasticPoco poco) : base(poco) { }
        
        public static Update Document<T>(T poco) where T: IElasticPoco
        {
            return new Update(poco);
        }
    }
}
