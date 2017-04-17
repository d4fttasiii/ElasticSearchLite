using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Bulk : IQuery
    {
        public string IndexName { get; }
        internal List<(ElasticMethods Method, IElasticPoco Poco)> PocosAndMethods { get; }

        protected Bulk(string indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            PocosAndMethods = new List<(ElasticMethods Method, IElasticPoco Poco)>();
        }
    }

    public class Bulk<T> : Bulk where T : IElasticPoco
    {
        protected Bulk(string indexName) : base(indexName) { }

        public static Bulk<T> Create(string indexName)
        {
            return new Bulk<T>(indexName);
        }

        public Bulk<T> Index(params T[] pocos)
        {
            CheckPocos(pocos);
            PocosAndMethods.AddRange(pocos.Select(poco => (ElasticMethods.Index, poco as IElasticPoco)));

            return this;
        }

        public Bulk<T> Update(params T[] pocos)
        {
            CheckPocos(pocos);
            PocosAndMethods.AddRange(pocos.Select(poco => (ElasticMethods.Update, poco as IElasticPoco)));

            return this;
        }

        public Bulk<T> Delete(params T[] pocos)
        {
            CheckPocos(pocos);
            PocosAndMethods.AddRange(pocos.Select(poco => (ElasticMethods.Delete, poco as IElasticPoco)));

            return this;
        }

        private void CheckPocos(T[] pocos)
        {
            if (pocos == null) { throw new ArgumentNullException(nameof(pocos)); }
            if (!pocos.Any()) { throw new ArgumentException(nameof(pocos)); }
        }
    }
}
