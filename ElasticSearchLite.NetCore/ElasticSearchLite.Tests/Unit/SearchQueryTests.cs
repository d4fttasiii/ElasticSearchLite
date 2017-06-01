using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                .Return<Poco>()
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
            TestQuery(statementObject, query, true);
        }

        [TestMethod]
        public void SearchQuery_Generation_Match()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<Poco>()
                .Match(p => p.TestText, "ABCD EFGH")
                    .Operator(ElasticOperators.And)
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
            TestQuery(statementObject, query, true);
        }

        [TestMethod]
        public void SearchQuery_Generation_MatchPhrase()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<Poco>()
                .MatchPhrase(p => p.TestText, "ABCD EFGH")
                .Take(10)
                .Skip(10);
            var statementObject = new
            {
                query = new
                {
                    match_phrase = new { TestText = "ABCD EFGH" }
                },
                size = 10,
                from = 10
            };

            // Act and Assert
            TestQuery(statementObject, query);
            TestQuery(statementObject, query, true);
        }

        [TestMethod]
        public void SearchQuery_Generation_MatchPhrasePrefix()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<Poco>()
                .MatchPhrasePrefix(p => p.TestText, "ABCD EFGH")
                .Take(10)
                .Skip(10);
            var statementObject = new
            {
                query = new
                {
                    match_phrase_prefix = new { TestText = "ABCD EFGH" }
                },
                size = 10,
                from = 10
            };

            // Act and Assert
            TestQuery(statementObject, query);
            TestQuery(statementObject, query, true);
        }

        //[TestMethod]
        //public void SearchQuery_Generation_MultiMatch()
        //{
        //    // Arrange
        //    var query = Search.In("mypocoindex")
        //        .Return<ComplexPoco>()
        //        .MultiMatch("ABCD", p => p.Name, p => p.Description)
        //        .Take(10)
        //        .Skip(10);
        //    var statementObject = new
        //    {
        //        query = new
        //        {
        //            multi_match = new
        //            {
        //                query = "ABCD",
        //                fields = new string[2]
        //                {
        //                    "Name",
        //                    "Description"
        //                }
        //            }
        //        },
        //        size = 10,
        //        from = 10
        //    };

        //    // Act and Assert
        //    TestQuery(statementObject, query);
        //    TestQuery(statementObject, query, true);
        //}

        [TestMethod]
        public void SearchQuery_Generation_Range()
        {
            // Arrange
            var query = Search.In("mypocoindex")
                .Return<Poco>()
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
            TestQuery(statementObject, query, true);
        }
    }
}
