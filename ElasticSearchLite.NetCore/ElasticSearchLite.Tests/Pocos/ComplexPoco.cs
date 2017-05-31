using ElasticSearchLite.NetCore.Interfaces;
using System;

namespace ElasticSearchLite.Tests.Pocos
{
    public class ComplexPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }
        public Tag Tag { get; set; }
        public Position Position { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class Tag
    {
        public string Name { get; set; }
        public string Summary { get; set; }
    }

    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
