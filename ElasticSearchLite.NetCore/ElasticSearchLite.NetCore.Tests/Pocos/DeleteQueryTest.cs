using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.NetCore.Queries.Condition;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.NetCore.Queries.Models;
using NUnit.Framework;

namespace ElasticSearchLite.NetCore.Tests.Pocos
{
    [TestFixture]
    public class DeleteQueryTest
    {
        private readonly IStatementGenerator generator = new StatementGenerator();

        [Test]
        public void BuildDeleteQuery()
        {
            // Arrange
            var poco = new TestPoco();
            var query = new DeleteQuery<TestPoco>(poco)
                .Term(new ElasticCodition() { Field = new ElasticField { Name = "Id" }, Value = "1" });

            // Act
            var postData = generator.Generate(query);

            // Assert
            // TODO : hack to fluent assertion in! :D
        }
    }
}
