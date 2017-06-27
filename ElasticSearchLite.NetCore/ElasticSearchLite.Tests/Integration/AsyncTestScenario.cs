using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass, TestCategory("Integration")]
    public class AsyncTestScenario : AbstractIntegrationScenario
    {
        [TestMethod]
        public void AsyncTestScenario_Index_Search_Update_Verify_Delete()
        {
            RunAsyncTestScenario_Index_Search_Update_Verify_Delete();
        }

        private async void RunAsyncTestScenario_Index_Search_Update_Verify_Delete()
        {
            Index.Document(poco)
                .ExecuteWith(_client);

            Thread.Sleep(2000);

            var document = (await Search
                .In(_indexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteAsyncWith(_client))
                .Single();

            poco.Id.ShouldBeEquivalentTo(document.Id);
            poco.Index.ShouldBeEquivalentTo(document.Index);
            poco.Type.ShouldBeEquivalentTo(document.Type);
            poco.TestBool.ShouldBeEquivalentTo(document.TestBool);
            poco.TestDouble.ShouldBeEquivalentTo(document.TestDouble);
            poco.TestInteger.ShouldBeEquivalentTo(document.TestInteger);
            poco.TestText.ShouldBeEquivalentTo(document.TestText);
            string.Join("", poco.TestStringArray).ShouldBeEquivalentTo(string.Join("", document.TestStringArray));

            poco.TestText = "ChangedText";
            Update.Document(poco).ExecuteAsyncWith(_client);

            Thread.Sleep(2000);

            (await Search.In(_indexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteAsyncWith(_client))
                .Single()
                .TestText
                .ShouldBeEquivalentTo(poco.TestText);

            (await Delete.Document(poco).ExecuteAsyncWith(_client)).Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void AsyncTestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            Bulk<Poco>
                 .Create(_indexName)
                 .Index(poco)
                 .Delete(poco)
                 .ExecuteAsyncWith(_client);
        }
    }
}
