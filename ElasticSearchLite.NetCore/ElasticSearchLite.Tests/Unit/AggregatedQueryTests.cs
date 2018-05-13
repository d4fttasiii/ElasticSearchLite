using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class AggregatedQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void AggregatedQuery_Avg_Test()
        {
            // Arrange
            var query = Aggregate.In<Poco>()
                .MatchAll()
                .Average(p => p.TestDouble);

            var statementObject = new
            {
                aggs = new
                {
                    avg_TestDouble = new
                    {
                        avg = new
                        {
                            field = "testDouble"
                        }
                    }
                },
                size = 0
            };

            TestQueryObject(statementObject, query, true);
        }

        [TestMethod]
        public void AggregatedQuery_Filtered_Avg_Test()
        {
            // Arrange
            var query = Aggregate.In<Poco>()
                .Term(p => p.TestText, "ABCDEFG")
                .Average(p => p.TestDouble);

            var statementObject = new
            {
                query = new
                {
                    terms = new { testText = new[] { "ABCDEFG" } }
                },
                aggs = new
                {
                    avg_TestDouble = new
                    {
                        avg = new
                        {
                            field = "testDouble"
                        }
                    }
                },
                size = 0
            };

            TestQueryObject(statementObject, query, true);
        }

        [TestMethod]
        public void AggregatedQuery_MovingAverage()
        {
            // Arrange
            var query = Aggregate.In<Poco>()
                .MatchAll()
                .EWMA(p => p.TestDouble)
                    .SetWindow(30)
                    .SetAlpha(0.3f);

            var statementObject = new
            {
                query = new
                {
                    terms = new { testText = new[] { "ABCDEFG" } }
                },
                aggs = new
                {
                    avg_TestDouble = new
                    {
                        avg = new
                        {
                            field = "testDouble"
                        }
                    }
                },
                size = 0
            };

            TestQueryObject(statementObject, query, true);
        }
    }
}
