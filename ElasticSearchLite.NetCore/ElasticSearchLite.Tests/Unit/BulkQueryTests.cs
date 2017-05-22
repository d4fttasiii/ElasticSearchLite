using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElasticSearchLite.Tests.Unit
{
    [TestClass]
    [TestCategory("Unit")]
    public class BulkQueryTests : AbstractQueryTest
    {
        [TestMethod]
        public void BulkQuery_Generation()
        {
            InitPoco();
            var query = Bulk<Poco>.Create("mypocoindex")
                .Index(poco)
                .Delete(poco);
            var statement = StatementFactory.Generate(query);

            statement.ShouldBeEquivalentTo(@"{ ""index"": { ""_id"": ""Id-1337"", ""_index"": ""mypocoindex"", ""_type"": ""mypoco"" } }
{ ""TestText"": ""ABCDEFG"",""TestInteger"": 12345,""TestDouble"": 1.337,""TestDateTime"": ""2017-09-10 00:00:00"",""TestBool"": true }
{ ""delete"": { ""_id"": ""Id-1337"", ""_index"": ""mypocoindex"", ""_type"": ""mypoco"" } }
", "Generated statement differs");
        }
    }
}
