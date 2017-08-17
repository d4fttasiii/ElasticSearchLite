using ElasticSearchLite.NetCore.Interfaces;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ElasticSearchLite.NetCore.Queries.Serialization
{
    public class JsonElasticPropertyResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly List<string> _elasticProperties = new List<string>
        {
            nameof(IElasticPoco.Id),
            nameof(IElasticPoco.Id).ToLower(),
            nameof(IElasticPoco.Index),
            nameof(IElasticPoco.Index).ToLower(),
            nameof(IElasticPoco.Type),
            nameof(IElasticPoco.Type).ToLower(),
            nameof(IElasticPoco.Total),
            nameof(IElasticPoco.Total).ToLower(),
            nameof(IElasticPoco.Version),
            nameof(IElasticPoco.Version).ToLower(),
            nameof(IElasticPoco.Score),
            nameof(IElasticPoco.Score).ToLower()
        };

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return objectType.GetProperties()
                .Where(pi => !_elasticProperties.Contains(pi.Name))
                .ToList<MemberInfo>();
        }
    }
}
