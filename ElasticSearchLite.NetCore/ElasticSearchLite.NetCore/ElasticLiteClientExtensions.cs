using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ElasticSearchLite.NetCore.Queries.Get;

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
            where TPoco : IElasticPoco => client.ExecuteGet(getQuery);
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
        /// <param name="update"></param>
        /// <param name="client"></param>
        public static async void ExecuteAsyncWith(this Update update, ElasticLiteClient client) => await client.ExecuteUpdateAsync(update);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="client"></param>
        public static async Task<int> ExecuteAsyncWith<TPoco>(this IDeleteExecutable<TPoco> delete, ElasticLiteClient client)
            where TPoco : IElasticPoco => await client.ExecuteDeleteAsync(delete);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="client"></param>
        public static async void ExecuteAsyncWith(this Index index, ElasticLiteClient client) => await client.ExecuteIndexAsync(index);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drop"></param>
        /// <param name="client"></param>
        public static async void ExecuteAsyncWith(this Drop drop, ElasticLiteClient client) => await client.ExecuteDropAsync(drop);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulk"></param>
        /// <param name="client"></param>
        public static async void ExecuteAsyncWith(this Bulk bulk, ElasticLiteClient client) => await client.ExecuteBulkAsync(bulk);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="client"></param>
        public static async Task<IEnumerable<TPoco>> ExecuteAsyncWith<TPoco>(this IExecutableSearchQuery<TPoco> search, ElasticLiteClient client)
            where TPoco : IElasticPoco => await client.ExecuteSearchAsync(search);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TPoco"></typeparam>
        /// <param name="getQuery"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Task<TPoco> ExecuteAsyncWith<TPoco>(this GetQuery<TPoco> getQuery, ElasticLiteClient client)
            where TPoco : IElasticPoco => client.ExecuteGetAsync(getQuery);
    }
}
