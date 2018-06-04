﻿using ElasticSearchLite.NetCore.Attributes;
using ElasticSearchLite.NetCore.Interfaces;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ElasticSearchLite.NetCore.Serialization
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
            var properties = objectType.GetProperties();

            return properties
                .Where(pi => !objectType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IElasticPoco)) || !_elasticProperties.Contains(pi.Name) || Attribute.IsDefined(pi, typeof(IgnoreAttribute)))
                .ToList<MemberInfo>();
        }
    }
}
