using System.Collections.Generic;
using System.Linq;
using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class IndexingScenario : AbstractIntegrationScenario
    {
        [TestMethod]
        public void IndexDocument_With_NestedObject_Containing_Elastic_FieldNames()
        {
            var document = new NestedTestPoco
            {
                Id = "1",
                Index = _indexName,
                Type = _typeName,
                Heroes = new List<Hero>
                {
                    new Hero { Id = "1", Name ="Tony Stark", Score = 9.9f}
                }
            };
            Index.Document(document).ExecuteWith(_client);

            Get.FromIndex(_indexName).Return<NestedTestPoco>(_typeName).ById("1").ExecuteWith(_client).Heroes.First().Id.Should().Be("1");
        }

        [TestCleanup]
        public void Cleanup()
        {
            Drop.Index(_indexName);
        }
    }
}
