using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries;
using System;
using ElasticSearchLite.Tests.Pocos;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class BoolQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void BoolQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentException), () => Bool.QueryIn(null), "Index name is null");
        }

        [TestMethod]
        public void BoolQuery_Building_Fluently()
        {
            // Arrange
            var query = Bool.QueryIn("mypocoindex")
                .Returns<Poco>()
                .Should(t => t.TestText)
                    .Match("something")
                .Should(t => t.TestText)
                    .Match("other thingy")
                .MustNot(t => t.TestDateTime)
                    .Range(ElasticRangeOperations.Lt, DateTime.Now);
        }

        [TestMethod]
        public void BoolQuery_Building_With_ComplexPoco_Fluently()
        {
            // Arrange
            var query = Bool.QueryIn("mypocoindex")
                .Returns<ComplexPoco>()
                .Should(t => t.Position.X)
                    .Match(1);

            // TestQueryString("", query, true);
        }
    }
}
