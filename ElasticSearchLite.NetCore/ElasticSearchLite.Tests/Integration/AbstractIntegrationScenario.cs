using ElasticSearchLite.NetCore;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using System;

namespace ElasticSearchLite.Tests.Integration
{
    public class AbstractIntegrationScenario
    {
        protected static readonly string _indexName = "exampleindex";
        protected static readonly string _typeName = "example";
        protected static readonly ElasticLiteClient _client = new ElasticLiteClient("http://bsv-dev-web001:9200/")
        {
            NameingStrategy = new Newtonsoft.Json.Serialization.CamelCaseNamingStrategy()
        };

        protected static readonly Poco poco = new Poco
        {
            Index = _indexName,
            Type = _typeName,
            TestInteger = 12345,
            TestText = "ABCDEFG",
            TestBool = true,
            TestDateTime = new DateTime(2017, 9, 10),
            TestDouble = 1.337,
            TestStringArray = new[] { "A", "B", "C", "D" }
        };

        protected static readonly EnumPoco enumPoco = new EnumPoco
        {
            Index = "tagindex",
            Type = "tag",
            TagType = TagType.A,
            Name = "Name1",
            Notes = "Notes right?"
        };

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
