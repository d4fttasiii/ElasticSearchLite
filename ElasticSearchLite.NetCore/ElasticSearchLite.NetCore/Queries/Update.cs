using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Update : AbstractBaseQuery
    {
        protected Update(IElasticPoco poco) : base(poco) { }
        /// <summary>
        /// Creates an update statement for the given poco
        /// It used the Update API 
        /// https://www.elastic.co/guide/en/elasticsearch/reference/5.4/docs-update.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <returns></returns>
        public static Update Document<T>(T poco) 
            where T: IElasticPoco => new Update(poco);
    }
}
