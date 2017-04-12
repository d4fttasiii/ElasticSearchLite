using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System.Linq;
using System;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class AbstractQuery : IQuery
    {
        internal ElasticCodition TermCondition { get; set; }
        internal ElasticCodition MatchCondition { get; set; }
        internal ElasticRangeCondition RangeCondition { get; set; }
        internal IElasticPoco Poco { get; set; }
        internal string IndexName { get; set; }
        internal string TypeName { get; set; }

        protected AbstractQuery(IElasticPoco poco)
        {
            Poco = poco ?? throw new ArgumentNullException(nameof(poco));
            IndexName = poco.Index;
            TypeName = poco.Type;
        }

        protected AbstractQuery(string indexName, string typeName)
        {
            if (string.IsNullOrEmpty(indexName)) { throw new ArgumentNullException(nameof(indexName)); }
            if (string.IsNullOrEmpty(typeName)) { throw new ArgumentNullException(nameof(typeName)); }

            IndexName = indexName;
            TypeName = typeName;
        }

        protected PT CheckParameter<PT>(PT parameter)
        {
            if (parameter == null) { throw new ArgumentNullException(nameof(parameter)); }
            return parameter;
        }

        protected void CheckParameters<PT>(PT[] parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            if (!parameters.Any()) { throw new ArgumentException(nameof(parameters)); }
        }

        protected abstract void ClearAllConditions();
    }
}
