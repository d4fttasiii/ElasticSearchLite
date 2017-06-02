namespace ElasticSearchLite.NetCore.Models
{
    public class ElasticMapping
    {
        public string Name { get; internal set; }
        public ElasticCoreFieldDataTypes FieldDataType { get; internal set; }
        public ElasticAnalyzers Analyzer { get; internal set; }
        public ElasticAnalyzerConfiguration AnalyzerConfiguration { get; internal set; }
    }
}
