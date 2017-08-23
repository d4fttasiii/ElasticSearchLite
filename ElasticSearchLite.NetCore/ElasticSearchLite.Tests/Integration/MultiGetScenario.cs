using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class MultiGetScenario : AbstractIntegrationScenario
    {
        [TestInitialize]
        public void Init()
        {
            for (int i = 0; i < 100; i++)
            {
                Index.Document(new Poco
                {
                    Id = $"{i}",
                    Index = "textindex",
                    Type = "text",
                    TestText = $"{Math.Pow(i, 2.0f)}",
                    TestInteger = i
                }).ExecuteWith(_client);
            }
            Thread.Sleep(1000);
        }

        [TestMethod]
        public void MultiGet_GetByMultipleIds_Returns_10_Documents()
        {
            var ids = new[] { "1", "3", "7", "11", "13", "17", "19", "23" };
            var documents = MGet.FromIndex("textindex")
                .Returns<Poco>()
                .ByIds(ids)
                .ExecuteWith(_client);

            documents.Count().Should().Be(8);
            documents.Select(d => d.Id).OrderBy(id => id).ToArray().Should().BeEquivalentTo(ids);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Drop.Index("textindex").ExecuteWith(_client);
        }
    }
}
