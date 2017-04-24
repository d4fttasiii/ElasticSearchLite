using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class DeleteQueryTest : AbstractQueryTest
    {
        [TestMethod]
        public void DeleteQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(A.Fake<MyPoco>()), "Poco is null");
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
        public void DeleteQuery_ExceptionTest_PocoId()
        {
            InitPoco();
            poco.Id = null;
            TestExceptions(typeof(ArgumentNullException), () => Delete.Document(poco), "Poco Id is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.From(null, A.Dummy<string>()), "Index name is null");
        }

        [TestMethod]
        public void DeleteQuery_ExceptionTest_Type()
        {
            TestExceptions(typeof(ArgumentNullException), () => Delete.From(A.Dummy<string>(), null), "Type name null");
        }

        [TestMethod]
        public void DeleteQuery_Generation_Term()
        {
            InitPoco();
            var query = Delete.From("mypocoindex", "mypoco")
                .Term("_id", "123");
            var queryObject = new
            {
                query = new
                {
                    term = new
                    {
                        _id = "123"
                    }
                }
            };

            TestQuery(queryObject, query);
        }

        [TestMethod]
        public void DeleteQuery_Generation_Match()
        {
            InitPoco();
            var query = Delete.From("mypocoindex", "mypoco")
                .Match("_id", "123");
            var queryObject = new
            {
                query = new
                {
                    match = new
                    {
                        _id = "123"
                    }
                }
            };

            TestQuery(queryObject, query);
        }

        [TestMethod]
        public void DeleteQuery_Generation_Range()
        {
            InitPoco();
            var query = Delete.From("mypocoindex", "mypoco")
                .Range("_id", ElasticRangeOperations.Gte, "123");
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

            TestQuery(queryObject, query);
        }
    }
}
