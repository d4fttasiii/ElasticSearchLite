using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Queries.Generator;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Newtonsoft.Json;
using System;

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
        
        protected void TestQueryObject<T>(T statementObject, IQuery query, bool camelCase = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (camelCase)
            {
                StatementFactory.NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy();
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }
            else
            {
                StatementFactory.NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy();
            }
            // Act
            var queryStatement = StatementFactory.Generate(query);

            // Assert
            statementObject.ShouldBeEquivalentTo(JsonConvert.DeserializeAnonymousType(queryStatement, statementObject, settings));
        }

        protected void TestQueryString(string statement, IQuery query, bool camelCase = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            if (camelCase)
            {
                StatementFactory.NamingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy();
                settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            }
            else
            {
                StatementFactory.NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy();
            }
            // Act
            var queryStatement = StatementFactory.Generate(query);

            statement.ShouldBeEquivalentTo(statement);
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
