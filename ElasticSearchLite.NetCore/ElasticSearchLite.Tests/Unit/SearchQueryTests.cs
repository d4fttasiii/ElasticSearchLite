using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.Tests.Poco;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class SearchQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void SearchQueryGeneration()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco");
            var statementObject = new { _source = true };

            // Act and Assert
            TestQuery(statementObject, query);
        }

        [TestMethod]
        public void SearchQueryGeneration_Term()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Term("TestText", "ABCDEFG");
            var statementObject = new
            {
                _source = true,
                query = new
                {
                    term = new { TestText = "ABCDEFG" }
                }
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }

        [TestMethod]
        public void SearchQueryGeneration_Match()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Match("TestText", "ABCDEFG");
            var statementObject = new
            {
                _source = true,
                query = new
                {
                    match = new { TestText = "ABCDEFG" }
                }
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }

        [TestMethod]
        public void SearchQueryGeneration_Range()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Range(ElasticFields.Id.Name, RangeOperations.Gt, 1);
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
                }
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }
    }
}
