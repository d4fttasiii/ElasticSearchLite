using ElasticSearchLite.NetCore.Interfaces;
using System;

namespace ElasticSearchLite.Tests.Pocos
{
    public class Poco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }
        public long Total { get; set; }
        public long Version { get; set; }

        public string TestText { get; set; }
        public int TestInteger { get; set; }
        public double TestDouble { get; set; }
        public DateTime TestDateTime { get; set; }
        public bool TestBool { get; set; }
        public string[] TestStringArray { get; set; }
    }
}
