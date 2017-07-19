using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ElasticSearchLite.NetCore.Models;
using Newtonsoft.Json.Serialization;

namespace ElasticSearchLite.NetCore.Queries.Serialization
{
    public class JsonElasticPropertyResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly List<string> _elasticProperties = new List<string>
        {
            ElasticFields.Id.Name,
            ElasticFields.Index.Name,
            ElasticFields.Score.Name,
            ElasticFields.Type.Name,
            ElasticFields.Total.Name
        };

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return objectType.GetProperties()
                .Where(pi => !_elasticProperties.Contains(pi.Name))
                .ToList<MemberInfo>();
        }
    }
}
