using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class CreateQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void SimpleCreate_Test()
        {
            var create = Create.Index("blaindex")
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

            TestQuery(new { }, create);
        }
    }
}
