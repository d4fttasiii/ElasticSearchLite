namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMatchCodition : ElasticTermCodition
    {
        public ElasticOperators Operation { get; set; }
    }
}
