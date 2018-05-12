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

            poco.Id.Should().Be(document.Id);
            poco.Index.Should().Be(document.Index);
            poco.Type.Should().Be(document.Type);
            poco.TestBool.Should().Be(document.TestBool);
            poco.TestDouble.Should().Be(document.TestDouble);
            poco.TestInteger.Should().Be(document.TestInteger);
            poco.TestText.Should().Be(document.TestText);
            string.Join("", poco.TestStringArray).Should().Be(string.Join("", document.TestStringArray));

            poco.TestText = "ChangedText";
            Update.Document(poco).ExecuteWith(_client);

            Thread.Sleep(2000);

            Search.In(_indexName)
                .Return<Poco>()
                .Term(p => p.Id, poco.Id)
                .ExecuteWith(_client)
                .Single()
                .TestText
                .Should().Be(poco.TestText);

            Delete.Document(poco).ExecuteWith(_client).Should().BeGreaterThan(0);
            _client.ExecuteDrop(Drop.Index(poco.Index));
        }

        [TestMethod]
        public void TestScenario_Bulk_Index_Delete()
        {
            poco.Id = "1337";

            _client.ExecuteBulk(Bulk
                .Create<Poco>(poco.Index)
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
            laPoco.Name.Should().Be(enumPoco.Name);
            laPoco.TagType.Should().Be(enumPoco.TagType);


            _client.ExecuteDrop(Drop.Index(enumPoco.Index));
        }
    }
}
