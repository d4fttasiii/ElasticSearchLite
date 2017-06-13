namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMatchPhraseCondition
    {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
        public ElasticOperators Operation { get; set; }
        public int Slop { get; set; }
    }
}
