using Newtonsoft.Json;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Generator;
using FluentAssertions;
using System;
using ElasticSearchLite.Tests.Pocos;

namespace ElasticSearchLite.Tests.Unit
{
    public abstract class AbstractQueryTest
    {
        protected IStatementFactory StatementFactory { get; } = new StatementFactory();

        protected Poco poco = new Poco();

        protected void InitPoco()
        {
            poco = new Poco
            {
                Id = "Id-1337",
                Index = "mypocoindex",
                Type = "mypoco",
                TestInteger = 12345,
                TestText = "ABCDEFG",
                TestBool = true,
                TestDateTime = new DateTime(2017, 9, 10),
                TestDouble = 1.337
            };
        }

        protected void TestQuery<T>(T statementObject, IQuery query)
        {
            // Act
            var queryStatement = StatementFactory.Generate(query);

            // Assert
            statementObject.ShouldBeEquivalentTo(JsonConvert.DeserializeAnonymousType(queryStatement, statementObject));
        }

        protected void TestExceptions(Type exception, Action action, string becauseMessage)
        {
            try
            {
                // Act
                action();
            }
            catch (Exception ex)
            {
                // Assert
                ex.GetType().ShouldBeEquivalentTo(exception, becauseMessage);
            }
        }
    }
}
