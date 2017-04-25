namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticTermCodition
    {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
    }

    public class ElasticMatchCodition : ElasticTermCodition { }
}
