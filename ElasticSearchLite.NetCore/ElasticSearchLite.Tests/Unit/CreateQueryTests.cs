using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class CreateQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void Create_Test()
        {
            var create = Create.Index("blaindex", "bla")
                .SetReplicasTo(1)
                .SetShardsTo(5)
                .DisableDynamicMapping()
                .EnableAllField()
                .AddMappings()
                    .AddMapping("name")
                        .WithType(ElasticCoreFieldDataTypes.Keyword)
                        .AddFieldAnalyzer(ElasticAnalyzers.Simple)
                        .WithoutConfiguration()
                    .AddMapping("description")
                        .WithType(ElasticCoreFieldDataTypes.Text)
                        .AddFieldAnalyzer(ElasticAnalyzers.Language)
                        .WithConfiguration(new ElasticAnalyzerConfiguration { Language = "german" })
                    .AddMapping("price")
                        .WithType(ElasticCoreFieldDataTypes.HalfFloat)
                    .AddMapping("newProduct")
                        .WithType(ElasticCoreFieldDataTypes.Boolean)
                    .AddMapping("sku")
                        .WithType(ElasticCoreFieldDataTypes.Keyword)
                        .AddFieldAnalyzer(ElasticAnalyzers.Fingerprint)
                        .WithConfiguration(new ElasticAnalyzerConfiguration { })
                    .AddMapping("in_stock", false)
                        .WithType(ElasticCoreFieldDataTypes.Short)
                    .AddMapping("next_shipment")
                        .WithType(ElasticCoreFieldDataTypes.Date);

            TestQueryObject(new
            {
                settings = new
                {
                    number_of_replicas = 1,
                    number_of_shards = 5
                },
                mappings = new
                {
                    bla = new
                    {
                        _all = new
                        {
                            enabled = false
                        },
                        properties = new
                        {
                            name = new
                            {
                                type = "keyword",
                                analyzer = "simple"
                            },
                            description = new
                            {
                                type = "text",
                                analyzer = "german"
                            },
                            price = new
                            {
                                type = "half_float"
                            },
                            new_product = new
                            {
                                type = "boolean"
                            },
                            sku = new
                            {
                                type = "keyword",
                                analyzer = "fingerprint"
                            },
                            in_stock = new
                            {
                                type = "short",
                                indexed = false
                            },
                            next_shipment = new
                            {
                                type = "date"
                            }
                        }
                    }
                }
            }, create);
        }
    }
}
