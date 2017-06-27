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
    public class TestScenario : AbstractIntegrationScenario
    {
        [TestMethod]
        public void TestScenario_Index_Search_Update_Verify_Delete()
        {
            Index.Document(poco)
                .ExecuteWith(_client);

            Thread.Sleep(2000);

            var document = Search
                .In(_indexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteWith(_client)
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
            Update.Document(poco).ExecuteWith(_client);

            Thread.Sleep(2000);

            Search.In(_indexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteWith(_client)
                .Single()
                .TestText
                .ShouldBeEquivalentTo(poco.TestText);

            Delete.Document(poco).ExecuteWith(_client).Should().BeGreaterThan(0);
            _client.ExecuteDrop(Drop.Index(poco.Index));
        }

        [TestMethod]
        public void TestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            _client.ExecuteBulk(Bulk<Poco>
                .Create(poco.Index)
                .Index(poco)
                .Delete(poco));

            Thread.Sleep(2000);

            Search.In(poco.Index)
                .Return<Poco>()
                .Term(p => p.Id, "1337")
                .ExecuteWith(_client)
                .FirstOrDefault()
                .Should().BeNull();

            _client.ExecuteDrop(Drop.Index(poco.Index));
        }

        [TestMethod]
        public void TestScenario_Enum_Index_Select()
        {
            Index.Document(enumPoco).ExecuteWith(_client);

            Thread.Sleep(2000);

            var laPoco = Search.In(enumPoco.Index)
                .Return<EnumPoco>()
                .ExecuteWith(_client)
                .FirstOrDefault();

            laPoco.Should().NotBeNull();
            laPoco.Name.ShouldBeEquivalentTo(enumPoco.Name);
            laPoco.TagType.ShouldBeEquivalentTo(enumPoco.TagType);


            _client.ExecuteDrop(Drop.Index(enumPoco.Index));
        }
    }
}
