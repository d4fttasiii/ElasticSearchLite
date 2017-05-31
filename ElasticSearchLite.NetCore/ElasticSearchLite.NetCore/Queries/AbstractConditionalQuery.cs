using ElasticSearchLite.NetCore.Interfaces;
using ElasticSearchLite.NetCore.Models;
using System.Collections.Generic;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class AbstractConditionalQuery : AbstractBaseQuery
    {
        internal List<ElasticRangeCondition> RangeConditions { get; } = new List<ElasticRangeCondition>();
        internal List<ElasticTermCodition> TermConditions { get; } = new List<ElasticTermCodition>();
        internal ElasticMultiMatchCondition MultiMatchConditions { get; set; }
        internal ElasticMatchCodition MatchCondition { get; set; }
        internal ElasticMatchCodition MatchPhraseCondition { get; set; }
        internal ElasticMatchCodition MatchPhrasePrefixCondition { get; set; }

        protected AbstractConditionalQuery(IElasticPoco poco) : base(poco) { }

        protected AbstractConditionalQuery(string indexName) : base(indexName) { }

        protected AbstractConditionalQuery(string indexName, string typeName) : base(indexName, typeName) { }
    }
}
