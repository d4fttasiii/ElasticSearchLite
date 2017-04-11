using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using ElasticSearchLite.NetCore.Queries.Models;
using ElasticSearchLite.NetCore.Queries.Condition;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class SearchQueryTests
    {
        IStatementGenerator Generator { get; } = new StatementGenerator();

        [TestMethod]
        public void SearchQueryGeneration()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco");
        }

        [TestMethod]
        public void SearchQueryGeneration_Term()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Term("TestText", "ABCDEFG");
        }

        [TestMethod]
        public void SearchQueryGeneration_Match()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Match("TestText", "ABCDEFG");
        }

        //[TestMethod]
        //public void SearchQueryGeneration_Matches()
        //{
        //    // Arrange
        //    var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
        //        .Matches("TestText", "ABCDEFG");
        //}

        [TestMethod]
        public void SearchQueryGeneration_Range()
        {
            // Arrange
            var query = SearchQuery<MyPoco>.Create("mypocoindex", "mypoco")
                .Range(ElasticFields.Id.Name, RangeOperations.Gt, 1);
        }
    }
}
