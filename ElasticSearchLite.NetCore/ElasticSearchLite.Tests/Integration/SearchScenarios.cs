using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Models;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class SearchScenarios : AbstractIntegrationScenario
    {
        private List<ComplexPoco> complexPocos = new List<ComplexPoco>();
        private static int Count = 100;
        private static string complexIndexName = "complexpocoindex";

        [TestInitialize]
        public void Init()
        {
            for (int i = 0; i < Count; i++)
            {
                complexPocos.Add(new ComplexPoco
                {
                    Id = $"ID-{i}",
                    Index = complexIndexName,
                    Type = "complexpoco",
                    Name = $"ComplexPoco - {i}",
                    LastModified = new DateTime(2017, 9, 10).AddDays(i),
                    Position = new Position
                    {
                        X = 5 * i,
                        Y = 5 * i
                    },
                    Tag = new Tag
                    {
                        Name = $"Tag-{i}",
                        Summary = $"Summary-{i}"
                    }
                });
            }
        }

        [TestMethod]
        public void SearchScenario_Add100Pocos_Get100Poco()
        {
            // Pocos Added
            foreach (var poco in complexPocos)
            {
                Index.Document(poco).ExecuteWith(Client);
            }

            var complexPocosReturned = Search.In(complexIndexName)
                .Return<ComplexPoco>()
                .Take(100)
                .Skip(0)
                .ExecuteWith(Client);

            foreach (var poco in complexPocosReturned)
            {
                poco.Id.Should().NotBeNull();
                poco.LastModified.Should().BeAfter(new DateTime(2017, 9, 9));
                poco.Position.Should().NotBeNull();
                poco.Tag.Should().NotBeNull();
            }
        }

        [TestCleanup]
        public void CleanUp()
        {
            Drop.Index(complexIndexName);
        }
    }
}
