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
    [TestClass, TestCategory("Integration")]
    public class BoolScenarios : AbstractIntegrationScenario
    {
        [TestMethod]
        public void BoolScenario_Search_For_MatchPhrasePrefix()
        {
            var texts = Bool.QueryIn("textindex")
                .Returns<Poco>()
                .Sources(p => p.TestText, p => p.TestInteger)
                .Should(p => p.TestText)
                    .MatchPhrasePrefix("1")
                .Take(100)
                .Skip(0)
                .Sort(p => p.TestInteger)
                    .Ascending()
                    .WithoutKeyword()
                .ExecuteWith(_client);

            texts.Should().NotBeNull();
            texts.Count().Should().Be(20);
        }

        [TestMethod]
        public void BoolScenario_Search_Using_Keyword_Sorting()
        {
            var texts = Bool.QueryIn("textindex")
                .Returns<Poco>()
                .Take(20)
                .Skip(0)
                .Sort(p => p.TestText)
                    .Descending()
                    .WithKeyword()
                .ExecuteWith(_client);

            texts.Should().NotBeNull();
            texts.Count().Should().Be(20);
            texts.First().Poco.Id.Should().NotBeNullOrWhiteSpace();
            texts.First().Poco.TestText.Should().BeEquivalentTo("9801");
        }



        [TestMethod]
        public void BoolScenario_Search_Terms()
        {
            var texts = Bool.QueryIn("textindex")
                .Returns<Poco>()
                .Should(p => p.TestText)
                    .Term("9801")
                .ExecuteWith(_client);

            texts.Should().NotBeNull();
            texts.First().Poco.Id.Should().NotBeNullOrWhiteSpace();
            texts.First().Poco.TestText.Should().BeEquivalentTo("9801");
        }

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

        [TestCleanup]
        public void CleanUp()
        {
            Drop.Index("textindex").ExecuteWith(_client);
        }
    }
}
