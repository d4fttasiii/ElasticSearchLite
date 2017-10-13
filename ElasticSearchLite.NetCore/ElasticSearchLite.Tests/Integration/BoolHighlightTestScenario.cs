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
    public class BoolHighlightTestScenario : AbstractIntegrationScenario
    {
        [TestMethod]
        public void BoolQuery_Highlight_Search_For_MatchPhrasePrefix()
        {
            var highlights = Bool.QueryIn("textindex")
                .Returns<Poco>()
                .EnableHighlighting()
                    .AddField(p => p.TestText)
                    .SetPreTagTo("<b>")
                    .SetPostTagTo("</b>")
                    .LimitTheNumberOfFragmentsTo(3)
                    .LimitFragmentSizeTo(100)
                .Should(p => p.TestText)
                    .MatchPhrasePrefix("1")
                .Take(10)
                .Skip(0)
                .ExecuteWith(_client);

            highlights.Should().NotBeNull();
            highlights.Count().Should().Be(10);
            highlights.First().Poco.Total.Should().Be(20);
            highlights.SelectMany(h =>
                h.Highlight
                .Values
                .SelectMany(v => v))
                .ToList()
                .ForEach(h =>
                {
                    h.Contains("1")
                        .Should()
                        .BeTrue();
                });
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
