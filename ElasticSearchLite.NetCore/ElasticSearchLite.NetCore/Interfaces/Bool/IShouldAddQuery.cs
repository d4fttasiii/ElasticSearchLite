using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Interfaces.Bool
{
    public interface IShouldAddQuery<TPoco> where TPoco : IElasticPoco
    {
        IExecutableBoolQuery<TPoco> Match(object value);
        IExecutableBoolQuery<TPoco> MatchPhrase(object value);
        IExecutableBoolQuery<TPoco> MatchPhrasePrefix(object value);
    }
}
