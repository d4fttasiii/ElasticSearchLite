namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMatchPhraseCondition : ElasticMatchCodition
    {
        public int Slop { get; set; }
    }
}
