using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.Tests.Poco;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class SearchQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void SearchQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentException), () => Search.In(null), "Index name is null");
        }

        [TestMethod]
        public void SearchQuery_Generation_Term()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<MyPoco>()
                .Term(p => p.TestText, "ABCDEFG")
                    .Or("GFEDCBA")
                .Take(15)
                .Skip(15);
            var statementObject = new
            {
                query = new
                {
                    terms = new { TestText = new[] { "ABCDEFG", "GFEDCBA" } }
                },
                size = 15,
                from = 15
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }

        [TestMethod]
        public void SearchQuery_Generation_Match()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<MyPoco>()
                .Match(p => p.TestText, "ABCDEFG")
                .Take(10)
                .Skip(10);
            var statementObject = new
            {
                query = new
                {
                    match = new { TestText = "ABCDEFG" }
                },
                size = 10,
                from = 10
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }

        [TestMethod]
        public void SearchQuery_Generation_Range()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<MyPoco>()
                .Range(p => p.Id, ElasticRangeOperations.Gt, 1)
                    .And(ElasticRangeOperations.Lt, 10)
                .Take(20);
            var statementObject = new
            {
                query = new
                {
                    range = new
                    {
                        _id = new
                        {
                            gt = 1,
                            lt = 10
                        }
                    }
                },
                size = 20,
                from = 0
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }
    }
}
