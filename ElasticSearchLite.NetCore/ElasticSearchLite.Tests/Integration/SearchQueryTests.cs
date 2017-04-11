using ElasticSearchLite.NetCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    [TestCategory("Integration")]
    public class SearchQueryTests
    {
        private ElasticLiteClient Client { get; } = new ElasticLiteClient("http://localhost:9200");

        [TestInitialize]
        public void InitSearchQueryTest()
        {

        }

        [TestCleanup]
        public void CleanUpSearchQueryTest()
        {

        }
    }
}
