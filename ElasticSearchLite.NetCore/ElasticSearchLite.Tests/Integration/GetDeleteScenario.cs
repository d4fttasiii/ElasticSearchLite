using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass, TestCategory("Integration")]
    public class GetDeleteScenario : AbstractIntegrationScenario
    {
        private ComplexPoco _complexPoco = new ComplexPoco
        {
            Id = $"ID-1337",
            Index = complexIndexName,
            Type = "complexpoco",
            Name = $"ComplexPoco - 1337",
            LastModified = new DateTime(2017, 9, 10).AddDays(1337),
            Position = new Position
            {
                X = 1337,
                Y = 1337
            },
            Tag = new Tag
            {
                Name = $"Tag-1337",
                Summary = $"Summary-1337"
            }
        };
        private static int Count = 100;
        private static string complexIndexName = "complexpocoindex";

        [TestMethod]
        public void Get_Delete_Scenario()
        {
            Index.Document(_complexPoco).ExecuteWith(_client);

            var document = Get.FromIndex(complexIndexName)
                .Return<ComplexPoco>("complexpoco")
                .ById(_complexPoco.Id)
                .ExecuteWith(_client);

            document.Should().NotBeNull();

            Delete.From(complexIndexName).Documents<ComplexPoco>().Term(c => c.Id, document.Id).ExecuteWith(_client).Should().Be(1);
        }        
    }
}
