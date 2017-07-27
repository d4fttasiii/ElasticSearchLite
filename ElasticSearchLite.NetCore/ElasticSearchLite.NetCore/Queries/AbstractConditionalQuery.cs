using ElasticSearchLite.NetCore.Interfaces;
using System.Collections.Generic;
using ElasticSearchLite.NetCore.Models.Conditions;

namespace ElasticSearchLite.NetCore.Queries
{
    public abstract class AbstractConditionalQuery : AbstractBaseQuery
    {
        internal List<ElasticRangeCondition> RangeConditions { get; } = new List<ElasticRangeCondition>();
        internal ElasticTermCodition TermCondition { get; set; }
        internal ElasticMultiMatchCondition MultiMatchConditions { get; set; }
        internal ElasticMatchCodition MatchCondition { get; set; }
        internal ElasticMatchPhraseCondition MatchPhraseCondition { get; set; }
        internal ElasticMatchPhrasePrefixCondition MatchPhrasePrefixCondition { get; set; }

        protected AbstractConditionalQuery(IElasticPoco poco) : base(poco) { }

        protected AbstractConditionalQuery(string indexName) : base(indexName) { }

        protected AbstractConditionalQuery(string indexName, string typeName) : base(indexName, typeName) { }
    }
}
