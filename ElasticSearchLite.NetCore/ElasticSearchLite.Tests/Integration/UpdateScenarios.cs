using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Exceptions;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class UpdateScenarios : AbstractIntegrationScenario
    {
        [TestMethod]
        public void Update_ViolatesVersion_Throws_Exception()
        {
            var poco = new Poco
            {
                Index = _indexName,
                Type = _typeName,
                Id = "Update_Test_1337",
                TestText = "ABCDEFG"
            };

            Index.Document(poco).ExecuteWith(_client);

            poco.Version -= 1;

            try
            {
                Update.Document(poco).ExecuteWith(_client);
            }
            catch (Exception ex)
            {
                ex.GetType().Should().Be(typeof(VersionConflictException));
            }
        }
    }
}
