using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.Tests.Poco;
using System;
using FakeItEasy;

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
                .ThatReturns<MyPoco>()
                .Term("TestText", "ABCDEFG")
                .Take(15)
                .Skip(15);
            var statementObject = new
            {
                query = new
                {
                    term = new { TestText = "ABCDEFG" }
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
                .ThatReturns<MyPoco>()
                .Match("TestText", "ABCDEFG")
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
                .ThatReturns<MyPoco>()
                .Range(ElasticFields.Id.Name, ElasticRangeOperations.Gt, 1);
            var statementObject = new
            {
                query = new
                {
                    range = new
                    {
                        _id = new
                        {
                            gt = 1
                        }
                    }
                },
                size = 25,
                from = 0
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }
    }
}
