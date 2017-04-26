using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Interfaces.Search;
using ElasticSearchLite.NetCore.Queries;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore
{
    public static class ElasticLiteClientExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="client"></param>
        public static void ExecuteUsing(this Update update, ElasticLiteClient client) => client.ExecuteUpdate(update);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="delete"></param>
        /// <param name="client"></param>
        public static int ExecuteUsing(this IDeleteExecutable delete, ElasticLiteClient client) => client.ExecuteDelete(delete);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="client"></param>
        public static void ExecuteUsing(this Index index, ElasticLiteClient client) => client.ExecuteIndex(index);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drop"></param>
        /// <param name="client"></param>
        public static void ExecuteUsing(this Drop drop, ElasticLiteClient client) => client.ExecuteDrop(drop);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bulk"></param>
        /// <param name="client"></param>
        public static void ExecuteUsing(this Bulk bulk, ElasticLiteClient client) => client.ExecuteBulk(bulk);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="client"></param>
        public static IEnumerable<TPoco> ExecuteUsing<TPoco>(this IExecutableSearchQuery<TPoco> search, ElasticLiteClient client) where TPoco : IElasticPoco => client.ExecuteSearch(search);
    }
}
