using ElasticSearchLite.NetCore.Interfaces;
using System;

namespace ElasticSearchLite.NetCore.Tests.Pocos
{
    public class TestPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double Score { get; set; }

        public string TestField1 { get; set; }
        public DateTime TestField2 { get; set; }
    }
}
