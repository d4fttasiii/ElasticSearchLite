using ElasticSearchLite.NetCore.Interfaces.Create;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Create
    {
        public static IConfigurableCreate Index(string indexName) => new CreateQuery(indexName);

        public sealed class CreateQuery 
            : AbstractBaseQuery,
            IConfigurableCreate,
            IMappableCreate,
            IMappingAddedCreate,
            IMappingWithTypeAddedCreate,
            IFieldAnalyserAddedCreate
        {
            internal int NumberOfShards { get; private set; } = 5;
            internal int NumberOfReplicas { get; private set; } = 1;
            internal bool AllFieldEnabled { get; private set; }
            internal bool DynamicMappingEnabled { get; private set; }
            internal List<ElasticMapping> Mapping { get; }
            private ElasticMapping TempMapping { get; set; }

            internal CreateQuery(string indexName) : base(indexName)
            {
                Mapping = new List<ElasticMapping>();
            }

            /// <summary>
            /// Setting the number of primary shards for and index. 
            /// Default is 5.
            /// </summary>
            /// <param name="shards"></param>
            /// <returns></returns>
            public IConfigurableCreate SetShardsTo(int shards)
            {
                if (shards < 0)
                {
                    throw new ArgumentException("number_of_shards should be at least 1");
                }
                NumberOfShards = shards;

                return this;
            }
            /// <summary>
            /// Setting the number of replicas for every shard, which determines the number of primary shard copies in the cluster.           
            /// Increasing this value will lead to overall bigger index sizes, but it also improves parallel read performance.
            /// Default is 1.
            /// </summary>
            /// <param name="replicas"></param>
            /// <returns></returns>
            public IConfigurableCreate SetReplicasTo(int replicas)
            {
                if (replicas < 0)
                {
                    throw new ArgumentException("number_of_replicas should be at least 1");
                }
                NumberOfReplicas = replicas;

                return this;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate EnableDynamicMapping()
            {
                DynamicMappingEnabled = true;

                return this;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate DisableDynamicMapping()
            {
                DynamicMappingEnabled = false;

                return this;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate EnableAllField()
            {
                AllFieldEnabled = true;

                return this;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate DisableAllField()
            {
                AllFieldEnabled = false;

                return this;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IMappableCreate AddMappings()
            {
                return this;
            }
            public IMappingAddedCreate AddMapping(string name, bool indexed = true)
            {
                if(string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }
                TempMapping = new ElasticMapping { Name = name };

                return this;
            }
            public IMappingWithTypeAddedCreate WithType(ElasticCoreFieldDataTypes type)
            {
                TempMapping.FieldDataType = type ?? throw new ArgumentNullException(nameof(type));

                return this;
            }
            public IFieldAnalyserAddedCreate AddFieldAnalyzer(ElasticAnalyzers analyzer)
            {
                TempMapping.Analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));

                return this;
            }
            public IMappableCreate WithConfiguration(ElasticAnalyzerConfiguration configuration)
            {
                TempMapping.AnalyzerConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));

                return this;
            }
            public IMappableCreate WithoutConfiguration()
            {
                return this;
            }
        }
    }
}
