using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class TestScenario
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
        public void TestScenario_Index_Search_Update_Verify_Delete()
        {
            var indexQuery = Index<MyPoco>.Document(poco);
            Client.ExecuteIndex(indexQuery);

            Thread.Sleep(2000);

            var searchQuery = Search<MyPoco>.In(indexName, typeName).Term(ElasticFields.Id.Name, poco.Id);
            var document = Client.ExecuteSearch(searchQuery).SingleOrDefault();

            Assert.AreEqual(poco.Id, document.Id);
            Assert.AreEqual(poco.Index, document.Index);
            Assert.AreEqual(poco.Type, document.Type);
            Assert.AreEqual(poco.TestBool, document.TestBool);
            Assert.AreEqual(poco.TestDouble, document.TestDouble);
            Assert.AreEqual(poco.TestInteger, document.TestInteger);
            Assert.AreEqual(poco.TestText, document.TestText);
            Assert.AreEqual(string.Join("", poco.TestStringArray), string.Join("", document.TestStringArray));

            poco.TestText = "ChangedText";
            Client.ExecuteUpdate(Update<MyPoco>.Document(poco));

            Thread.Sleep(2000);

            document = Client.ExecuteSearch(searchQuery).FirstOrDefault();
            Assert.AreEqual(poco.TestText, document.TestText);

            var deleteQuery = Delete<MyPoco>.Document(poco).Term(ElasticFields.Id.Name, poco.Id);
            Client.ExecuteDelete(deleteQuery);
        }

        [TestMethod]
        public void TestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            Client.ExecuteBulk(Bulk<MyPoco>
                .Create("mypocoindex")
                .Index(poco)
                .Delete(poco));
        }

        [TestCleanup]
        public void CleanUp()
        {
            Client.ExecuteDrop(Drop.Index(poco.Index));
        }
    }
}
