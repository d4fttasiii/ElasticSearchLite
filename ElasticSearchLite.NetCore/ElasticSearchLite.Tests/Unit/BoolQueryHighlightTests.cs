using ElasticSearchLite.NetCore.Models.Enums;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class BoolQueryHighlightTests : AbstractQueryTest
    {
        [TestMethod]
        public void BoolQuery_With_Highlight_Fluently()
        {
            // Arrange
            var query = Bool.QueryIn("mypocoindex")
                .Returns<Poco>()
                .ShouldMatchAtLeast(1)
                .Should(t => t.TestText)
                    .Match("something")
                .MustNot(t => t.TestDateTime)
                    .Range(ElasticRangeOperations.Lt, DateTime.Now)
                .EnableHighlighting()
                    .AddField(t => t.TestText)
                    .SetPreTagTo("<b>")
                    .SetPostTagTo("</b>")
                    .LimitTheNumberOfFragmentsTo(3)
                    .LimitFragmentSizeTo(150)                    
                ;

            // TestQueryString("", query);
        }
    }
}
