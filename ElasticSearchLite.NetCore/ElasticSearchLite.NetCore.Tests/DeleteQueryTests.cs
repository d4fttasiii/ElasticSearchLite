using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Queries.Condition;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.NetCore.Queries.Models;
using ElasticSearchLite.NetCore.Tests.Pocos;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Tests
{
    [TestFixture]
    public class DeleteQueryTests
    {
        private IStatementGenerator Generator { get; } = new StatementGenerator();

        [Test]
        public void GenerateDeleteQuery_Range()
        {
            // Arrange
            var query = SearchQuery<TestPoco>.Create("testIndex", "test")
                .Term(new ElasticCodition
                {
                    Field = new ElasticField { Name = "Key" },
                    Value = "1"
                });

            // Act
            var statement = Generator.Generate(query);
        }
    }
}
