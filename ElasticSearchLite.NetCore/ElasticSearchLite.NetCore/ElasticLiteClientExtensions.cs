using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Bool;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using System.Collections.Generic;
using static ElasticSearchLite.NetCore.Queries.Get;
using static ElasticSearchLite.NetCore.Queries.MGet;

namespace ElasticSearchLite.NetCore
{
    public static class ElasticLiteClientExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="client"></param>
        public static void ExecuteWith(this Update update, ElasticLiteClient client) => client.ExecuteUpdate(update);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="client"></param>
        public static int ExecuteWith<TPoco>(this IDeleteExecutable<TPoco> delete, ElasticLiteClient client)
            where TPoco : IElasticPoco => client.ExecuteDelete(delete);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="client"></param>
        public static void ExecuteWith(this Index index, ElasticLiteClient client) => client.ExecuteIndex(index);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drop"></param>
        /// <param name="client"></param>
        public static void ExecuteWith(this Drop drop, ElasticLiteClient client) => client.ExecuteDrop(drop);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulk"></param>
        /// <param name="client"></param>
        public static void ExecuteWith(this Bulk bulk, ElasticLiteClient client) => client.ExecuteBulk(bulk);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="getQuery"></param>
        /// <param name="client"></param>
        public static TPoco ExecuteWith<TPoco>(this GetQuery<TPoco> getQuery, ElasticLiteClient client)
            where TPoco : class, IElasticPoco => client.ExecuteGet(getQuery);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="getQuery"></param>
        /// <param name="client"></param>
        public static IEnumerable<TPoco> ExecuteWith<TPoco>(this MultiGetQuery<TPoco> mgetQuery, ElasticLiteClient client)
            where TPoco : IElasticPoco => client.ExecuteMGet(mgetQuery);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="search"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IEnumerable<TPoco> ExecuteWith<TPoco>(this IExecutableSearchQuery<TPoco> search, ElasticLiteClient client)
            where TPoco : IElasticPoco => client.ExecuteSearch(search);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="boolQuery"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static IEnumerable<ElasticBoolResponse<TPoco>> ExecuteWith<TPoco>(this IBoolQueryExecutable<TPoco> boolQuery, ElasticLiteClient client)
            where TPoco : IElasticPoco => client.ExecuteBool(boolQuery);       
    }
}
