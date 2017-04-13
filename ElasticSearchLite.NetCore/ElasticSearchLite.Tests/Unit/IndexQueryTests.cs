using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class IndexQueryTests : AbstractQueryTest
    {
        private MyPoco poco = new MyPoco
        {
            TestInteger = 12345,
            TestText = "ABCDEFG",
            TestBool = true,
            TestDateTime = new DateTime(2017, 9, 10),
            TestDouble = 1.337,
            TestStringArray = new[] {"A", "B", "C", "D"}
        };
        
        [TestMethod]
        public void IndexQueryGeneration()
        {
            // Arrange
            var query = IndexQuery<MyPoco>.Create(poco);
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
