using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMatchPhraseCondition : ElasticMatchCodition
    {
        public int Slop { get; set; }
    }
}
