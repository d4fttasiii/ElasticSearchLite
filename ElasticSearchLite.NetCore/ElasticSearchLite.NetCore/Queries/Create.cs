using ElasticSearchLite.NetCore.Interfaces.Create;
using ElasticSearchLite.NetCore.Models;
using System;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Queries
{
    public class Create
    {
        public static IConfigurableCreate Index(string indexName, string typeName) => new CreateQuery(indexName, typeName);

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
            internal bool AllFieldEnabled { get; private set; } = true;
            internal bool DynamicMappingEnabled { get; private set; } = true;
            internal List<ElasticMapping> Mapping { get; }
            private ElasticMapping TempMapping { get; set; }

            internal CreateQuery(string indexName, string typeName) : base(indexName, typeName)
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
            /// Enable dynamic mapping for the index
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate EnableDynamicMapping()
            {
                DynamicMappingEnabled = true;

                return this;
            }
            /// <summary>
            /// Disable dynamic mapping for the index
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate DisableDynamicMapping()
            {
                DynamicMappingEnabled = false;

                return this;
            }
            /// <summary>
            /// Enable _all field
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate EnableAllField()
            {
                AllFieldEnabled = true;

                return this;
            }
            /// <summary>
            /// Disable _all field
            /// </summary>
            /// <returns></returns>
            public IConfigurableCreate DisableAllField()
            {
                AllFieldEnabled = false;

                return this;
            }
            /// <summary>
            /// Go into mapping state
            /// </summary>
            /// <returns></returns>
            public IMappableCreate AddMappings()
            {
                return this;
            }
            /// <summary>
            /// Adds a new field to the index
            /// </summary>
            /// <param name="name"></param>
            /// <param name="indexed"></param>
            /// <returns></returns>
            public IMappingAddedCreate AddMapping(string name, bool indexed = true)
            {
                if(string.IsNullOrEmpty(name)) { throw new ArgumentNullException(nameof(name)); }
                TempMapping = new ElasticMapping { Name = name };

                return this;
            }
            /// <summary>
            /// Defines the type for the previously added field
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public IMappingWithTypeAddedCreate WithType(ElasticCoreFieldDataTypes type)
            {
                TempMapping.FieldDataType = type ?? throw new ArgumentNullException(nameof(type));

                return this;
            }
            /// <summary>
            /// Adds analyzer to the field
            /// </summary>
            /// <param name="analyzer"></param>
            /// <returns></returns>
            public IFieldAnalyserAddedCreate AddFieldAnalyzer(ElasticAnalyzers analyzer)
            {
                TempMapping.Analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));

                return this;
            }
            /// <summary>
            /// Configures the previously added analyzer
            /// </summary>
            /// <param name="configuration"></param>
            /// <returns></returns>
            public IMappableCreate WithConfiguration(ElasticAnalyzerConfiguration configuration)
            {
                TempMapping.AnalyzerConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));

                return this;
            }
            /// <summary>
            /// Adds analyzer without configuration
            /// </summary>
            /// <returns></returns>
            public IMappableCreate WithoutConfiguration()
            {
                return this;
            }
        }
    }
}
