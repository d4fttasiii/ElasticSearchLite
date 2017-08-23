using ElasticSearchLite.NetCore.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class MultiGetQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void MultiGet_Generation()
        {
            var query = MGet.FromIndex("testindex")
                .Returns<Pocos.Poco>()
                .ByIds("1", "2");

            TestQueryObject(new
            {
                docs = new[] {
                    new
                    {
                        _id = "1"
                    },
                    new
                    {
                        _id = "2"
                    }
                }
            }, query);
        }
    }
}
