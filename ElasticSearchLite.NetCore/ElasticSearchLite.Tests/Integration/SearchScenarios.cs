﻿using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Exceptions;
using ElasticSearchLite.NetCore.Models.Enums;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass, TestCategory("Integration")]
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
                    },
                    TagNames = new string[] { $"ohh - {i}", $"bla - {i}" }
                });
            }
        }

        [TestMethod]
        public void SearchScenario_Add100Pocos_Get100Poco()
        {
            // Pocos Added
            foreach (var poco in complexPocos)
            {
                Index.Document(poco).ExecuteWith(_client);
            }

            var complexPocosReturned = Search.In(complexIndexName)
                .Return<ComplexPoco>()
                .Sort(cp => cp.LastModified, ElasticSortOrders.Ascending)
                .Take(100)
                .Skip(0)
                .ExecuteWith(_client);

            complexPocosReturned.Count().Should().Be(Count);

            foreach (var poco in complexPocosReturned)
            {
                poco.Id.Should().NotBeNull();
                poco.LastModified.Should().BeAfter(new DateTime(2017, 9, 9));
                poco.Position.Should().NotBeNull();
                poco.Tag.Should().NotBeNull();
            }

            var filteredPocos = Search.In(complexIndexName)
                .Return<ComplexPoco>()
                .Range(cp => cp.LastModified, ElasticRangeOperations.Gt, new DateTime(2017, 9, 15))
                    .And(ElasticRangeOperations.Lte, new DateTime(2017, 9, 20))
                .ExecuteWith(_client);

            foreach (var poco in filteredPocos)
            {
                poco.LastModified.Should().BeAfter(new DateTime(2017, 9, 14));
                poco.LastModified.Should().BeBefore(new DateTime(2017, 9, 21));
            }
        }

        [TestMethod]
        public void SearchScenario_Index_Doesnt_Exists()
        {
            TestExceptions(typeof(IndexNotAvailableException), () =>
            {
                Search.In("ishouldntexistsindex")
                    .Return<Poco>()
                    .ExecuteWith(_client);
            }, 
            "Index not available exception should be thrown.");

        }

        [TestCleanup]
        public void CleanUp()
        {
            Drop.Index(complexIndexName);
        }
    }
}
