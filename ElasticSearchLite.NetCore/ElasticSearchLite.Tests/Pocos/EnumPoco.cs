using ElasticSearchLite.NetCore.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ElasticSearchLite.Tests.Pocos
{
    public class EnumPoco : IElasticPoco
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Index { get; set; }
        public double? Score { get; set; }

        public TagType TagType { get; set; }
        public string Name { get; set; }
        public IList<string> ParentTags { get; set; }
        public string Notes { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TagType
    {
        [EnumMember(Value = "Content")]
        Content,
        [EnumMember(Value = "Category")]
        Category,
        [EnumMember(Value = "Cluster")]
        Cluster,
        [EnumMember(Value = "Theme")]
        Theme
    }
}
