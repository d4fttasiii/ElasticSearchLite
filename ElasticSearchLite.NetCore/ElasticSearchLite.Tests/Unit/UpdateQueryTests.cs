using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class UpdateQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void UpdateQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Update.Document(A.Fake<Poco>()), "Poco is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoIndex()
        {
            InitPoco();
            poco.Index = null;
            TestExceptions(typeof(ArgumentNullException), () => Update.Document(poco), "Poco index is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoType()
        {
            InitPoco();
            poco.Type = null;
            TestExceptions(typeof(ArgumentNullException), () => Update.Document(poco), "Poco type is null");
        }

        [TestMethod]
        public void UpdateQuery_ExceptionTest_PocoId()
        {
            InitPoco();
            poco.Id = null;
            TestExceptions(typeof(ArgumentNullException), () => Update.Document(poco), "Poco Id is null");
        }

        [TestMethod]
        public void UpdateQuery_Generation_UpdatePoco()
        {
            // Arrange
            InitPoco();
            var query = Update.Document(poco);
            var queryObject = new
            {
                doc = new
                {
                    TestInteger = 12345,
                    TestText = "ABCDEFG",
                    TestBool = true,
                    TestDateTime = new DateTime(2017, 9, 10),
                    TestDouble = 1.337
                }
            };

            // Act and Assert
            TestQueryObject(queryObject, query);
            TestQueryObject(queryObject, query, true);
        }
    }
}
