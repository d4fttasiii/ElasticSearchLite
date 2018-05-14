using ElasticSearchLite.NetCore;
using ElasticSearchLite.NetCore.Models.Enums;
using ElasticSearchLite.NetCore.Queries;
using ElasticSearchLite.Tests.Pocos;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElasticSearchLite.Tests.Integration
{
    [TestClass]
    public class AggregatedQuieryIntegrationScenario : AbstractIntegrationScenario
    {
        private readonly Random rnd = new Random();
        private List<Poco> pocos = new List<Poco>();

        [TestMethod]
        public void Simple_Aggregation_Sum_Test()
        {
            var expectedSum = pocos.Sum(p => p.TestDouble);
            var calculatedSum = Aggregate.In<Poco>("textindex")
                .MatchAll()
                .Sum(p => p.TestDouble)
                .ExecuteWith(_client)
                .First()
                .AggregatedValue;

            calculatedSum.Should().Be(calculatedSum);
        }

        [TestMethod]
        public void Simple_Aggregation_Moving_Average_Test()
        {
            var movingAverages = Aggregate.In<Poco>("textindex")
                .Range(p => p.TestDateTime, ElasticRangeOperations.Lt, new DateTime(2017, 1, 11))
                .SimpleMovingAverage(p => p.TestDouble)
                .SetDateHistogramField(p => p.TestDateTime)
                .SetDateHistogramInterval("1d")
                .SetWindow(30)
                .ExecuteWith(_client);

            movingAverages.Should().HaveCount(10);
        }

        [TestInitialize]
        public void Init()
        {
            for (int i = 0; i < 100; i++)
            {
                pocos.Add(new Poco
                {
                    Id = $"{i}",
                    Index = "textindex",
                    Type = "text",
                    TestText = $"{Math.Pow(i, 2.0f)}",
                    TestInteger = i,
                    TestDouble = rnd.NextDouble(),
                    TestDateTime = new DateTime(2017, 1, 1).AddDays(i)
                });
            }

            pocos.ForEach(poco => Index.Document(poco).ExecuteWith(_client));

            Thread.Sleep(1000);
        }
    }
}
