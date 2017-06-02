using ElasticSearchLite.NetCore.Models;

namespace ElasticSearchLite.NetCore.Interfaces.Create
{
    public interface IMappingWithTypeAddedCreate : IMappableCreate
    {
        IFieldAnalyserAddedCreate AddFieldAnalyzer(ElasticAnalyzers analyzer);
    }
}
