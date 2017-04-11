using ElasticSearchLite.NetCore.Interfaces;

namespace ElasticSearchLite.Tests.Poco
{
    public class MyPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double Score { get; set; }
        public string TestText { get; set; }
        public int TestInteger { get; set; }
    }
}
