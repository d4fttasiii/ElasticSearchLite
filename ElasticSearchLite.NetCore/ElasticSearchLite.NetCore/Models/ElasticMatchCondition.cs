namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMatchCodition
    {
        public ElasticField Field { get; set; }
        public object Value { get; set; }
        public ElasticOperators Operation { get; set; }
        public int Fuzziness { get; set; }
    }
}
