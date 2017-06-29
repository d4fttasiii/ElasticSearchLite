using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class HighlightTestScenario : AbstractIntegrationScenario
    {
        [TestMethod]
        public void Highlight_Search_For_MatchPhrasePrefix()
        {
            var highlights = Highlight.QueryIn("textindex")
                .Returns<Poco>()
                .WithPre("<b>")
                .WithPost("</b>")
                .AddFields(p => p.TestText)
                .Should(p => p.TestText)
                    .MatchPhrasePrefix("1")
                .Take(100)
                .Skip(0)
                .ExecuteWith(_client);

            highlights.Should().NotBeNull();
            highlights.Count().Should().Equals(20);
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
        }

        [TestCleanup]
        public void CleanUp()
        {
            Drop.Index("textindex").ExecuteWith(_client);
        }
    }
}
