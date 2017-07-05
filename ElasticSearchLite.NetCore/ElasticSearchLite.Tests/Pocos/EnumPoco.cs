using ElasticSearchLite.NetCore.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ElasticSearchLite.Tests.Pocos
{
    public class EnumPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }
        public long Total { get; set; }

        public TagType TagType { get; set; }
        public string Name { get; set; }
        public List<string> ParentTags { get; set; }
        public string Notes { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TagType
    {
        [EnumMember(Value = "A")]
        A,
        [EnumMember(Value = "B")]
        B,
        [EnumMember(Value = "C")]
        C,
        [EnumMember(Value = "D")]
        D
    }
}
