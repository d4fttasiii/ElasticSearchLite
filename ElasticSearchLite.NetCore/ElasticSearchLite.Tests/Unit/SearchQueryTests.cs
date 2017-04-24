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
            TestExceptions(typeof(ArgumentNullException), () => Search<MyPoco>.In(null, A.Dummy<string>()), "Index name is null");
        }

        [TestMethod]
        public void SearchQuery_ExceptionTest_Type()
        {
            TestExceptions(typeof(ArgumentNullException), () => Search<MyPoco>.In(A.Dummy<string>(), null), "Type name null");
        }

        [TestMethod]
        public void SearchQuery_Generation_Term()
        {
            // Arrange
            var query = Search<MyPoco>.In("mypocoindex", "mypoco")
                .Term("TestText", "ABCDEFG")
                .Take(15)
                .Skip(15);
            var statementObject = new
            {
                _source = true,
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
            var query = Search<MyPoco>.In("mypocoindex", "mypoco")
                .Match("TestText", "ABCDEFG")
                .Skip(10)
                .Take(10);
            var statementObject = new
            {
                _source = true,
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
            var query = Search<MyPoco>.In("mypocoindex", "mypoco")
                .Range(ElasticFields.Id.Name, ElasticRangeOperations.Gt, 1);
            var statementObject = new
            {
                _source = true,
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
