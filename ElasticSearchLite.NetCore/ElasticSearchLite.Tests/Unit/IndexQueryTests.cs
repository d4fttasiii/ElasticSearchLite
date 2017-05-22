using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class IndexQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void IndexQuery_ExceptionTest_Poco()
        {
            TestExceptions(typeof(ArgumentNullException), () => Index.Document(A.Fake<Poco>()), "Poco is null");
        }

        [TestMethod]
        public void IndexQuery_ExceptionTest_PocoIndex()
        {
            InitPoco();
            poco.Index = null;
            TestExceptions(typeof(ArgumentNullException), () => Index.Document(poco), "Poco index is null");
        }

        [TestMethod]
        public void IndexQuery_ExceptionTest_PocoType()
        {
            InitPoco();
            poco.Type = null;
            TestExceptions(typeof(ArgumentNullException), () => Index.Document(poco), "Poco type is null");
        }

        [TestMethod]
        public void IndexQuery_Generation()
        {
            // Arrange
            InitPoco();
            var query = Index.Document(poco);
            var statementObject = new
            {
                TestInteger = 12345,
                TestText = "ABCDEFG",
                TestBool = true,
                TestDateTime = new DateTime(2017, 9, 10),
                TestDouble = 1.337
            };

            // Act and Assert
            TestQuery(statementObject, query);
        }
    }
}
