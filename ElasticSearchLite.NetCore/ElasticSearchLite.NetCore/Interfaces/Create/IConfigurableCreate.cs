namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IConfigurableCreate
    {
        /// <summary>
        /// Setting the number of primary shards for and index. 
        /// Default is 5.
        /// </summary>
        /// <param name="shards"></param>
        /// <returns></returns>
        IConfigurableCreate SetShardsTo(int shards);
        /// <summary>
        /// Setting the number of replicas for every shard, which determines the number of primary shard copies in the cluster.           
        /// Increasing this value will lead to overall bigger index sizes, but it also improves parallel read performance.
        /// Default is 1.
        /// </summary>
        /// <param name="replicas"></param>
        /// <returns></returns>
        IConfigurableCreate SetReplicasTo(int replicas);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigurableCreate EnableDynamicMapping();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigurableCreate DisableDynamicMapping();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigurableCreate EnableAllField();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConfigurableCreate DisableAllField();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IMappableCreate AddMappings();
    }
}
