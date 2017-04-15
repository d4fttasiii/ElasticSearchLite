using Newtonsoft.Json;
using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.NetCore.Queries;
using FluentAssertions;
using System;
using ElasticSearchLite.Tests.Poco;

namespace ElasticSearchLite.Tests.Unit
{
    public abstract class AbstractQueryTest
    {
        IStatementFactory Generator { get; } = new StatementFactory();

        protected MyPoco poco = new MyPoco();

        protected void InitPoco()
        {
            poco = new MyPoco
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

        protected void TestQuery<T>(T statementObject, AbstractQuery query)
        {
            // Act
            var queryStatement = Generator.Generate(query);

            // Assert
            statementObject.ShouldBeEquivalentTo(JsonConvert.DeserializeAnonymousType(queryStatement, statementObject));
        }

        protected void TestExceptions(Type exception, Action ctor, string becauseMessage)
        {
            try
            {
                // Act
                ctor();
            }
            catch (Exception ex)
            {
                // Assert
                ex.GetType().ShouldBeEquivalentTo(exception, becauseMessage);
            }
        }
    }
}
