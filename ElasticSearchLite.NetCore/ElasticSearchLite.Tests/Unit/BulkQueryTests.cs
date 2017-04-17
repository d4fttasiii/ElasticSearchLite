using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class BulkQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void BulkQuery_Generation()
        {
            InitPoco();
            var query = Bulk<MyPoco>.Create("mypocoindex")
                .Index(poco)
                .Delete(poco);
        }
    }
}
