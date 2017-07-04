using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class HighlightQuery : AbstractQueryTest
    {
        [TestMethod]
        public void BoolQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentException), () => Bool.QueryIn(null), "Index name is null");
        }

        [TestMethod]
        public void Highlight_Query_Fluently()
        {
            // Arrange
            var query = Highlight.QueryIn("mypocoindex")
                .Returns<Poco>()
                .WithPre("<b>")
                .WithPost("</b>")
                .AddFields(t => t.TestText, t => t.TestText)
                .Should(t => t.TestText)
                    .Match("something")
                .Should(t => t.TestText)
                    .Match("other thingy")
                .MustNot(t => t.TestDateTime)
                    .Range(ElasticRangeOperations.Lt, DateTime.Now);

            TestQueryString("", query);
        }
    }
}
