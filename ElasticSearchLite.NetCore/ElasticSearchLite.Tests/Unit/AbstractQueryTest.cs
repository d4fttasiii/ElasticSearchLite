using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Generator;

namespace ElasticSearchLite.Tests.Unit
{
    public abstract class AbstractQueryTest
    {
        IStatementFactory Generator { get; } = new StatementFactory();

        protected void TestQuery<T>(T statementObject, IQuery query)
        {
            // Act
            var queryStatement = Generator.Generate(query);

            // Assert
            Assert.AreEqual(statementObject, JsonConvert.DeserializeAnonymousType(queryStatement, statementObject));
        }
    }
}
