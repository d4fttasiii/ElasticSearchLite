using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class UpsertQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void UpsertQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Upsert.Document(A.Fake<Poco>()), "Poco is null");
        }

        [TestMethod]
        public void UpsertQuery_ExceptionTest_PocoIndex()
        {
            InitPoco();
            poco.Index = null;
            TestExceptions(typeof(ArgumentNullException), () => Upsert.Document(poco), "Poco index is null");
        }

        [TestMethod]
        public void UpsertQuery_ExceptionTest_PocoType()
        {
            InitPoco();
            poco.Type = null;
            TestExceptions(typeof(ArgumentNullException), () => Upsert.Document(poco), "Poco type is null");
        }

        [TestMethod]
        public void UpsertQuery_ExceptionTest_PocoId()
        {
            InitPoco();
            poco.Id = null;
            TestExceptions(typeof(ArgumentNullException), () => Upsert.Document(poco), "Poco Id is null");
        }

        [TestMethod]
        public void UpsertQuery_Generation_UpdatePoco()
        {
            // Arrange
            InitPoco();
            var query = Upsert.Document(poco);
            var queryObject = new
            {
                doc = new
                {
                    TestInteger = 12345,
                    TestText = "ABCDEFG",
                    TestBool = true,
                    TestDateTime = new DateTime(2017, 9, 10),
                    TestDouble = 1.337
                },
                doc_as_upsert = true
            };

            // Act and Assert
            TestQuery(queryObject, query);
        }
    }
}
