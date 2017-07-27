using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class DeleteQueryTest : AbstractQueryTest
    {
        [TestMethod]
        public void DeleteQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(A.Fake<Poco>()), "Poco is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_PocoIndex()
        {
            InitPoco();
            poco.Index = null;
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(poco), "Poco index is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_PocoType()
        {
            InitPoco();
            poco.Type = null;
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(poco), "Poco type is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_From()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.From(""), "Indexname should not be empty");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_PocoId()
        {
            InitPoco();
            poco.Id = null;
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(poco), "Poco Id is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.From(null), "Index name is null");
        }

        [TestMethod]
        public void DeleteQuery_Generation_Term()
        {
            InitPoco();
            var query = Delete
                .From("mypocoindex")
                .Documents<Poco>()
                .Term(p => p.Id, "123");

            var queryObject = new
            {
                query = new
                {
                    terms = new
                    {
                        _id = new[] { "123" }
                    }
                }
            };

            TestQueryObject(queryObject, query);
            TestQueryObject(queryObject, query, true);
        }

        [TestMethod]
        public void DeleteQuery_Generation_Match()
        {
            InitPoco();
            var query = Delete
                .From("mypocoindex")
                .Documents<Poco>()
                .Match(p => p.Id, "123");
           
            TestQueryString($@"{{ ""query"": {{ ""match"": {{ ""_id"" : {{ ""query"": ""123"", ""operator"": ""and"" }} }} }} }}", query);
        }

        [TestMethod]
        public void DeleteQuery_Generation_Range()
        {
            InitPoco();
            var query = Delete
                .From("mypocoindex")
                .Documents<Poco>()
                .Range(p => p.Id, ElasticRangeOperations.Gte, "123");
            var queryObject = new
            {
                query = new
                {
                    range = new
                    {
                        _id = new
                        {
                            gte = "123"
                        }
                    }
                }
            };

            TestQueryObject(queryObject, query);
            TestQueryObject(queryObject, query, true);
        }
    }
}
