using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using ElasticSearchLite.Tests.Pocos;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class BoolQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void BoolQuery_ExceptionTest_Index()
        {
            TestExceptions(typeof(ArgumentException), () => Bool.In(null), "Index name is null");
        }

        [TestMethod]
        public void BoolQuery_Building_Fluently()
        {
            // Arrange
            var query = Bool.In("mypocoindex")
                .Returns<Poco>()
                .Should(t => t.TestText).Match("something")
                .Should(t => t.TestText).Match("other thingy")
                .MustNot(t => t.TestDateTime).Range(ElasticRangeOperations.Lt, DateTime.Now);

            TestQueryString("", query);
        }
    }
}
