using ElasticSearchLite.NetCore.Interfaces;
using System.Collections.Generic;

namespace ElasticSearchLite.Tests.Pocos
{
    public class NestedTestPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }
        public long Total { get; set; }
        public long Version { get; set; }

        public List<Hero> Heroes { get; set; }
    }

    public class Hero
    {
        public string Id { get; set; }
        public double Score { get; set; }
        public string Name { get; set; }
    }
}
