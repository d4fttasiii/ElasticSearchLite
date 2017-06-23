using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestCategory("Integration")]
    [TestClass]
    public class TestScenario : AbstractIntegrationScenario
    {  
        [TestMethod]
        public void TestScenario_Index_Search_Update_Verify_Delete()
        {
            Index.Document(poco)
                .ExecuteWith(Client);

            Thread.Sleep(2000);

            var document = Search
                .In(IndexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteWith(Client)
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
            Update.Document(poco).ExecuteWith(Client);

            Thread.Sleep(2000);

            Search.In(IndexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteWith(Client)
                .Single()
                .TestText
                .ShouldBeEquivalentTo(poco.TestText);

            Delete.Document(poco).ExecuteWith(Client).Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void TestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            Client.ExecuteBulk(Bulk<Poco>
                .Create(IndexName)
                .Index(poco)
                .Delete(poco));
        }

        [TestMethod]
        public void TestScenario_Enum_Index_Select()
        {
            var laPoco = Search.In("tagindex")
                .Return<EnumPoco>()
                .ExecuteWith(Client)
                .FirstOrDefault();

            laPoco.Should().NotBeNull();
            laPoco.Name.ShouldBeEquivalentTo(enumPoco.Name);
            laPoco.TagType.ShouldBeEquivalentTo(enumPoco.TagType);
        }

        [TestCleanup]
        public void CleanUp()
        {
            Client.ExecuteDrop(Drop.Index(poco.Index));
        }
    }
}
