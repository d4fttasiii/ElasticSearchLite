using ElasticSearchLite.NetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ElasticSearchLite.NetCore.Models.Enums;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class Bulk : IQuery
    {
        internal string IndexName { get; }
        internal List<(ElasticMethods Method, IElasticPoco Poco)> PocosAndMethods { get; }

        protected Bulk(string indexName)
        {
            IndexName = indexName ?? throw new ArgumentNullException(nameof(indexName));
            PocosAndMethods = new List<(ElasticMethods Method, IElasticPoco Poco)>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public static Bulk<T> Create<T>(string indexName)
            where T: IElasticPoco
        {
            return new Bulk<T>(indexName);
        }
    }

    public class Bulk<T> : Bulk where T : IElasticPoco
    {
        protected internal Bulk(string indexName) : base(indexName) { }        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocos"></param>
        /// <returns></returns>
        public Bulk<T> Index(params T[] pocos)
        {
            CheckPocos(pocos);
            PocosAndMethods.AddRange(pocos.Select(poco => (ElasticMethods.Index, poco as IElasticPoco)));

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocos"></param>
        /// <returns></returns>
        public Bulk<T> Update(params T[] pocos)
        {
            CheckPocos(pocos);
            PocosAndMethods.AddRange(pocos.Select(poco => (ElasticMethods.Update, poco as IElasticPoco)));

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocos"></param>
        /// <returns></returns>
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
            if (pocos.Any(p => string.IsNullOrEmpty(p.Id))) { throw new ArgumentNullException($"{nameof(IElasticPoco.Id)} should be set for every Poco"); }
        }
    }
}
