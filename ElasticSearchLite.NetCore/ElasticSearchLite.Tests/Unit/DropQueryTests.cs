using ElasticSearchLite.NetCore.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class DropQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void DropQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentNullException), () => Drop.Index(null), "Index name is null");
        }
    }
}
