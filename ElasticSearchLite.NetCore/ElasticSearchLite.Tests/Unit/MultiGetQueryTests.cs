using ElasticSearchLite.NetCore.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    public class MultiGetQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void MultiGet_Generation_With_FieldSelection()
        {
            var query = MGet.FromIndex("testindex")
                .Returns<Pocos.Poco>()
                .SelectField(p => p.TestText)
                .SelectField(p => p.TestDouble)
                .ByIds("1", "2");

            TestQueryObject(new
            {
                docs = new[] {
                    new
                    {
                        _id = "1",
                        _source = new [] { "TestText", "TestDouble" }
                    },
                    new
                    {
                        _id = "2",
                        _source = new [] { "TestText", "TestDouble" }
                    }
                }
            }, query);
        }

        [TestMethod]
        public void MultiGet_Generation_Without_FieldSelection()
        {
            var query = MGet.FromIndex("testindex")
                .Returns<Pocos.Poco>()
                .ByIds("1", "2");

            TestQueryObject(new
            {
                docs = new[] {
                    new
                    {
                        _id = "1",
                        _source = "*"
                    },
                    new
                    {
                        _id = "2",
                        _source = "*"
                    }
                }
            }, query);
        }
    }
}
