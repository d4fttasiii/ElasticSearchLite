using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestCategory("Integration")]
    [TestClass]
    public class AsyncTestScenario
    {
        private static string indexName = "exampleindex";
        private static string typeName = "example";
        private ElasticLiteClient Client { get; } = new ElasticLiteClient("http://localhost:9200");
        private MyPoco poco = new MyPoco
        {
            Index = indexName,
            Type = typeName,
            TestInteger = 12345,
            TestText = "ABCDEFG",
            TestBool = true,
            TestDateTime = new DateTime(2017, 9, 10),
            TestDouble = 1.337,
            TestStringArray = new[] { "A", "B", "C", "D" }
        };

        [TestMethod]
        public void AsyncTestScenario_Index_Search_Update_Verify_Delete()
        {
            RunAsyncTestScenario_Index_Search_Update_Verify_Delete();
        }

        private async void RunAsyncTestScenario_Index_Search_Update_Verify_Delete()
        {
            Index.Document(poco)
                .ExecuteWith(Client);

            Thread.Sleep(2000);

            var document = (await Search
                .In(indexName)
                .Return<MyPoco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteAsyncWith(Client))
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
            Update.Document(poco).ExecuteAsyncWith(Client);

            Thread.Sleep(2000);

            (await Search.In(indexName)
                .Return<MyPoco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteAsyncWith(Client))
                .Single()
                .TestText
                .ShouldBeEquivalentTo(poco.TestText);

            (await Delete.Document(poco).ExecuteAsyncWith(Client)).Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void AsyncTestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            Bulk<MyPoco>
                 .Create("mypocoindex")
                 .Index(poco)
                 .Delete(poco)
                 .ExecuteAsyncWith(Client);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Client.ExecuteDrop(Drop.Index(poco.Index));
        }
    }
}
