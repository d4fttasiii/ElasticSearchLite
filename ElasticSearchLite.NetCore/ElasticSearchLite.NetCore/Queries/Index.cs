using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Index : AbstractQuery
    {
        protected Index(IElasticPoco poco) : base(poco) { }
        /// <summary>
        /// Create a document in an index associated with the poco.
        /// </summary>
        /// <param name="poco"></param>
        /// <returns></returns>
        public static Index Document<T>(T poco) where T : IElasticPoco
        {
            return new Index(poco);
        }
    }
}
